using UnityEngine;
using UnityEngine.Events;

namespace Client
{
    public class GameSession : MonoBehaviour
    {
        public static GameSession Instance { get; private set; }

        private const string LEVEL_PROGRESS_KEY = "LevelProgress_{0}_{1}";
        private const string LEVEL_SCORE_KEY = "LevelScore_{0}_{1}";
        private const string CORRECT_ANSWERS_KEY = "CorrectAnswers_{0}_{1}";
        private const string INCORRECT_ANSWERS_KEY = "IncorrectAnswers_{0}_{1}";
        private const string PLAYER_NAME_KEY = "PlayerName";
        
        public event UnityAction<int> ProfessionSelected;
        
        public int SelectedProfessionIndex
        {
            get
            {
                if (PlayerPrefs.HasKey("SelectedProfession"))
                {
                    return PlayerPrefs.GetInt("SelectedProfession");
                }

                PlayerPrefs.SetInt("SelectedProfession", 0);
                PlayerPrefs.Save();

                return PlayerPrefs.GetInt("SelectedProfession");
            }
            set
            {
                PlayerPrefs.SetInt("SelectedProfession", value);
                PlayerPrefs.Save();
            }
        }

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

            InitializePlayerPrefs();
        }

        public void SaveLevelProgress(TaskType taskType, int levelIndex, int score)
        {
            string progressKey = string.Format(LEVEL_PROGRESS_KEY, taskType, levelIndex);
            string scoreKey = string.Format(LEVEL_SCORE_KEY, taskType, levelIndex);
            string correctAnswersKey = string.Format(CORRECT_ANSWERS_KEY, taskType, levelIndex);
            string incorrectAnswersKey = string.Format(INCORRECT_ANSWERS_KEY, taskType, levelIndex);

            PlayerPrefs.SetInt(progressKey, 1);
            
            int currentScore = GetLevelScore(taskType, levelIndex);
            if (score > currentScore)
            {
                PlayerPrefs.SetInt(scoreKey, score);
            }
            
            // Запись правильных и неправильных ответов
            PlayerPrefs.SetInt(correctAnswersKey, PlayerPrefs.GetInt(correctAnswersKey, 0) + 1); // Увеличиваем количество правильных ответов
            PlayerPrefs.SetInt(incorrectAnswersKey, PlayerPrefs.GetInt(incorrectAnswersKey, 0)); // Увеличиваем количество неправильных ответов
            
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

        public int GetCorrectAnswers(TaskType taskType, int levelIndex)
        {
            string key = string.Format(CORRECT_ANSWERS_KEY, taskType, levelIndex);
            return PlayerPrefs.GetInt(key, 0);
        }

        public int GetIncorrectAnswers(TaskType taskType, int levelIndex)
        {
            string key = string.Format(INCORRECT_ANSWERS_KEY, taskType, levelIndex);
            return PlayerPrefs.GetInt(key, 0);
        }

        public void ResetProgress()
        {
            foreach (var taskType in System.Enum.GetValues(typeof(TaskType)))
            {
                for (int i = 0; i < 10; i++) // Предполагаем, что у нас 10 уровней
                {
                    string progressKey = string.Format(LEVEL_PROGRESS_KEY, taskType, i);
                    string scoreKey = string.Format(LEVEL_SCORE_KEY, taskType, i);
                    string correctAnswersKey = string.Format(CORRECT_ANSWERS_KEY, taskType, i);
                    string incorrectAnswersKey = string.Format(INCORRECT_ANSWERS_KEY, taskType, i);
                    
                    PlayerPrefs.DeleteKey(progressKey);
                    PlayerPrefs.DeleteKey(scoreKey);
                    PlayerPrefs.DeleteKey(correctAnswersKey);
                    PlayerPrefs.DeleteKey(incorrectAnswersKey);
                }
            }
            PlayerPrefs.Save();
        }
        
        private void InitializePlayerPrefs()
        {
            Debug.Log("InitializePlayerPrefs");

            if (!PlayerPrefs.HasKey("SelectedProfession"))
            {
                Debug.Log("New player prefs for professtion");
                PlayerPrefs.SetInt("SelectedProfession", SelectedProfessionIndex);
            }

            PlayerPrefs.Save();
        }

        public void SelectProfession(int professionIndex)
        {
            ProfessionSelected?.Invoke(professionIndex);
        }

        public void SavePlayerName(string playerName)
        {
            PlayerPrefs.SetString(PLAYER_NAME_KEY, playerName);
            PlayerPrefs.Save();
        }

        public string GetPlayerName()
        {
            return PlayerPrefs.GetString(PLAYER_NAME_KEY, "Player");
        }
    }
} 