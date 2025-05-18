using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client
{
    public class GameplaySceneManager : MonoBehaviour
    {
        [SerializeField] private GameplayManager _gameplayManager;
        [SerializeField] private BaseGameplayWindowView _miniCaseWindow;
        //private BaseGameplayWindowView _oddOneOutWindow;
        //[SerializeField] private BaseGameplayWindowView _pairMatchWindow;
        //[SerializeField] private BaseGameplayWindowView _sequenceWindow;

        private void Awake()
        {
            GetAllWindows();
        }

        private void Start()
        {
            // Get saved task type and level index
            TaskType taskType = (TaskType)PlayerPrefs.GetInt("CurrentTaskType");
            int levelIndex = PlayerPrefs.GetInt("CurrentLevelIndex");

            // Hide all windows
            _miniCaseWindow.gameObject.SetActive(false);
            //_oddOneOutWindow.gameObject.SetActive(false);
            //_pairMatchWindow.gameObject.SetActive(false);
            //_sequenceWindow.gameObject.SetActive(false);

            // Subscribe to gameplay events
            _gameplayManager.OnLevelCompleted += OnLevelCompleted;
            _gameplayManager.OnLevelFailed += OnLevelFailed;

            // Start level
            _gameplayManager.StartLevel(taskType, levelIndex);

            // Show appropriate window
            var taskData = _gameplayManager.GetCurrentTask();
            BaseGameplayWindowView window = GetWindowForTaskType(taskType);
            window.gameObject.SetActive(true);
            window.Initialize(taskData);
        }

        private void OnDestroy()
        {
            if (_gameplayManager != null)
            {
                _gameplayManager.OnLevelCompleted -= OnLevelCompleted;
                _gameplayManager.OnLevelFailed -= OnLevelFailed;
            }
        }

        private BaseGameplayWindowView GetWindowForTaskType(TaskType taskType)
        {
            switch (taskType)
            {
                case TaskType.MiniCase:
                    return _miniCaseWindow;
                default:
                    throw new System.ArgumentException($"Unknown task type: {taskType}");
            }
        }

        private void GetAllWindows()
        {
            _miniCaseWindow.GetComponentInChildren<MiniCaseWindowView>();
            //_oddOneOutWindow?.GetComponentInChildren<OddOneOutWindowView>();
        }

        private void OnLevelCompleted(int score)
        {
            // TODO: Show completion window with score
            Debug.Log($"Level completed with score: {score}");
            ReturnToMenu();
        }

        private void OnLevelFailed()
        {
            // TODO: Show failure window
            Debug.Log("Level failed");
            ReturnToMenu();
        }

        private void ReturnToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
} 