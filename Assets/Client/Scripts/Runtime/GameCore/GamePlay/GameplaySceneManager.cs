using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client
{
    public class GameplaySceneManager : MonoBehaviour
    {
        [SerializeField] private BaseGameplayWindowView _gameplayWindow;
        [SerializeField] private WinWindowView _winWindow;
        [SerializeField] private LoseWindowView _loseWindow;

        private void Start()
        {
            GameplayManager.Instance.OnTaskLoaded += OnTaskLoaded;
            GameplayManager.Instance.OnTaskCompleted += OnTaskCompleted;
            GameplayManager.Instance.OnTaskFailed += OnTaskFailed;

            var levels = LevelSystem.Instance.GetLevelsFor(LevelLoadParams.TaskType);
            if (levels == null || levels.Count == 0)
            {
                Debug.LogWarning("[GameplaySceneManager] No levels available, returning to main menu");
                SceneManager.LoadScene("MainMenu");
                return;
            }

            GameplayManager.Instance.StartTask(LevelLoadParams.TaskType, LevelLoadParams.LevelIndex, 0);
        }

        private void OnDestroy()
        {
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.OnTaskLoaded -= OnTaskLoaded;
                GameplayManager.Instance.OnTaskCompleted -= OnTaskCompleted;
                GameplayManager.Instance.OnTaskFailed -= OnTaskFailed;
            }
        }

        private void OnTaskLoaded(ITaskData taskData)
        {
            _gameplayWindow.Initialize(taskData);
            _gameplayWindow.Show();
            _winWindow.Close();
            _loseWindow.Close();
        }

        private void OnTaskCompleted()
        {
            _gameplayWindow.Hide();
            _winWindow.Initialize(
                GameplayManager.Instance.GetCorrectAnswersCount()
            );
            _winWindow.Open();
        }

        private void OnTaskFailed()
        {
            _gameplayWindow.Hide();
            _loseWindow.Open();
        }

        public void OnRetryButtonClick()
        {
            GameplayManager.Instance.StartTask(LevelLoadParams.TaskType, LevelLoadParams.LevelIndex, 0);
        }

        public void OnMenuButtonClick()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
} 