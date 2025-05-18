using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class LevelSystem : MonoBehaviour
    {
        public static LevelSystem Instance { get; private set; }

        [SerializeField] private List<LevelSetData> _allLevelSets;
        
        private const string LEVEL_PROGRESS_KEY = "LevelProgress_{0}_{1}"; // format: taskType_levelIndex
        private const string LEVEL_SCORE_KEY = "LevelScore_{0}_{1}";

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
            var levelSet = _allLevelSets.Find(set => set.taskType == taskType);
            if (levelSet == null) return null;

            // Update lock status based on progress
            for (int i = 0; i < levelSet.levels.Count; i++)
            {
                if (i == 0)
                {
                    levelSet.levels[i].IsLock = false; // First level always unlocked
                    continue;
                }

                // Level is unlocked if previous level is completed
                levelSet.levels[i].IsLock = !IsLevelCompleted(taskType, i - 1);
            }

            return levelSet.levels;
        }

        public void CompleteLevel(TaskType taskType, int levelIndex, int score = 0)
        {
            string progressKey = string.Format(LEVEL_PROGRESS_KEY, taskType, levelIndex);
            string scoreKey = string.Format(LEVEL_SCORE_KEY, taskType, levelIndex);

            PlayerPrefs.SetInt(progressKey, 1);
            
            // Save score only if it's higher than previous
            int currentScore = GetLevelScore(taskType, levelIndex);
            if (score > currentScore)
            {
                PlayerPrefs.SetInt(scoreKey, score);
            }
            
            PlayerPrefs.Save();
        }

        public bool IsLevelCompleted(TaskType taskType, int levelIndex)
        {
            string key = string.Format(LEVEL_PROGRESS_KEY, taskType, levelIndex);
            return PlayerPrefs.GetInt(key, 0) == 1;
        }

        public int GetLevelScore(TaskType taskType, int levelIndex)
        {
            string key = string.Format(LEVEL_SCORE_KEY, taskType, levelIndex);
            return PlayerPrefs.GetInt(key, 0);
        }

        public void ResetProgress()
        {
            foreach (var levelSet in _allLevelSets)
            {
                for (int i = 0; i < levelSet.levels.Count; i++)
                {
                    string progressKey = string.Format(LEVEL_PROGRESS_KEY, levelSet.taskType, i);
                    string scoreKey = string.Format(LEVEL_SCORE_KEY, levelSet.taskType, i);
                    
                    PlayerPrefs.DeleteKey(progressKey);
                    PlayerPrefs.DeleteKey(scoreKey);
                }
            }
            PlayerPrefs.Save();
        }
    }
}