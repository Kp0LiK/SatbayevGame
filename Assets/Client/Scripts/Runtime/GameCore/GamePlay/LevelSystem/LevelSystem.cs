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

        public List<Level> GetLevelsFor(TaskType taskType, Profession profession)
        {
            var taskSet = _allTaskSets.Find(set => set.TaskType == taskType && set.Profession == profession);
            if (taskSet == null) return null;

            return taskSet.Levels;
        }

        public Level GetLevel(TaskType taskType, Profession profession, int levelIndex)
        {
            var levels = GetLevelsFor(taskType, profession);
            if (levels == null || levelIndex >= levels.Count) return null;
            return levels[levelIndex];
        }

        public bool IsLevelAvailable(TaskType taskType, Profession profession, int levelIndex)
        {
            if (levelIndex == 0) return true;
            return IsLevelCompleted(taskType, profession, levelIndex - 1);
        }

        public void SaveLevelProgress(TaskType taskType, Profession profession, int levelIndex, int correctAnswers)
        {
            GameSession.Instance.SaveLevelProgress(taskType, profession, levelIndex, correctAnswers);
        }

        public bool IsLevelCompleted(TaskType taskType, Profession profession, int levelIndex)
        {
            return GameSession.Instance.IsLevelCompleted(taskType, profession, levelIndex);
        }

        public int GetLevelScore(TaskType taskType, Profession profession, int levelIndex)
        {
            return GameSession.Instance.GetLevelScore(taskType, profession, levelIndex);
        }

        public int GetCorrectAnswers(TaskType taskType, Profession profession, int levelIndex)
        {
            return GameSession.Instance.GetCorrectAnswers(taskType, profession, levelIndex);
        }

        public int GetIncorrectAnswers(TaskType taskType, Profession profession, int levelIndex)
        {
            return GameSession.Instance.GetIncorrectAnswers(taskType, profession, levelIndex);
        }

        public void ResetProgress()
        {
            GameSession.Instance.ResetProgress();
        }
    }
}