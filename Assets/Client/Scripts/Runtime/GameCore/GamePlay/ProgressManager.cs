using UnityEngine;
using System;

namespace Client
{
    public class ProgressManager : MonoBehaviour
    {
        public static ProgressManager Instance { get; private set; }

        public event Action<TaskType, int> OnLevelCompleted;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void CompleteLevel(TaskType taskType, int levelIndex, int correctAnswers)
        {
            string key = GetLevelKey(taskType, levelIndex);
            PlayerPrefs.SetInt(key, correctAnswers);
            PlayerPrefs.Save();
            
            OnLevelCompleted?.Invoke(taskType, levelIndex);
            Debug.Log($"[ProgressManager] Level completed: Type={taskType}, Level={levelIndex}, Score={correctAnswers}");
        }

        public bool IsLevelCompleted(TaskType taskType, int levelIndex)
        {
            string key = GetLevelKey(taskType, levelIndex);
            return PlayerPrefs.HasKey(key);
        }

        public int GetLevelScore(TaskType taskType, int levelIndex)
        {
            string key = GetLevelKey(taskType, levelIndex);
            return PlayerPrefs.GetInt(key, 0);
        }

        public void ClearProgress(TaskType taskType)
        {
            var levels = LevelSystem.Instance.GetLevelsFor(taskType);
            if (levels == null) return;

            for (int i = 0; i < levels.Count; i++)
            {
                string key = GetLevelKey(taskType, i);
                PlayerPrefs.DeleteKey(key);
            }
            PlayerPrefs.Save();
            Debug.Log($"[ProgressManager] Progress cleared for {taskType}");
        }

        private string GetLevelKey(TaskType taskType, int levelIndex)
        {
            return $"Level_{taskType}_{levelIndex}";
        }
    }
} 