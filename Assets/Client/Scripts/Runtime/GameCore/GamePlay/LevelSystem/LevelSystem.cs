using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class LevelSystem : MonoBehaviour
    {
        public static LevelSystem Instance { get; private set; }

        [SerializeField] private List<TaskSet> _allTaskSets;

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

        public List<Level> GetLevelsFor(TaskType taskType)
        {
            var taskSet = _allTaskSets.Find(set => set.TaskType == taskType);
            if (taskSet == null) return null;

            return taskSet.Levels;
        }

        public void CompleteLevel(TaskType taskType, int levelIndex, int correctAnswers)
        {
            GameSession.Instance.SaveLevelProgress(taskType, levelIndex, correctAnswers);
        }

        public bool IsLevelCompleted(TaskType taskType, int levelIndex)
        {
            return GameSession.Instance.IsLevelCompleted(taskType, levelIndex);
        }

        public int GetLevelScore(TaskType taskType, int levelIndex)
        {
            return GameSession.Instance.GetLevelScore(taskType, levelIndex);
        }

        public int GetCorrectAnswers(TaskType taskType, int levelIndex)
        {
            return GameSession.Instance.GetCorrectAnswers(taskType, levelIndex);
        }

        public int GetIncorrectAnswers(TaskType taskType, int levelIndex)
        {
            return GameSession.Instance.GetIncorrectAnswers(taskType, levelIndex);
        }

        public void ResetProgress()
        {
            GameSession.Instance.ResetProgress();
        }
    }
}