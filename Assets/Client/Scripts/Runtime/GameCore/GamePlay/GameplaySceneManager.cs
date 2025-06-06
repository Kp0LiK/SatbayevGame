using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client
{
    public class GameplaySceneManager : MonoBehaviour
    {
        [SerializeField] private MiniCaseWindowView _miniCaseWindow;
        [SerializeField] private OddOneOutWindowView _oddOneOutWindow;
        [SerializeField] private PairMatchWindowView _pairMatchWindow;
        [SerializeField] private WinWindowView _winWindow;
        [SerializeField] private LoseWindowView _loseWindow;

        private BaseGameplayWindowView _currentWindow;

        private void Start()
        {
            GameplayManager.Instance.OnTaskLoaded += OnTaskLoaded;
            GameplayManager.Instance.OnTaskCompleted += OnTaskCompleted;
            GameplayManager.Instance.OnTaskFailed += OnTaskFailed;

            var levels = LevelSystem.Instance.GetLevelsFor(GameplayManager.Instance.GetCurrentTaskType(),
                GameplayManager.Instance.GetCurrentProfession());
            if (levels == null || levels.Count == 0)
            {
                Debug.LogWarning("[GameplaySceneManager] No levels available, returning to main menu");
                SceneManager.LoadScene("MainMenu");
                return;
            }

            GameplayManager.Instance.StartTask(GameplayManager.Instance.GetCurrentTaskType(),
                GameplayManager.Instance.GetCurrentProfession(),
                GameplayManager.Instance.GetCurrentLevelIndex(), 0);
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
            if (_currentWindow != null)
                _currentWindow.Hide();

            _currentWindow = GetWindowForTask();
            _currentWindow.Initialize(taskData);
            _currentWindow.Show();

            _winWindow.Close();
            _loseWindow.Close();
        }

        private BaseGameplayWindowView GetWindowForTask()
        {
            switch (GameplayManager.Instance.GetCurrentTaskType())
            {
                case TaskType.MiniCase:
                    return _miniCaseWindow;
                case TaskType.OddOneOut:
                    return _oddOneOutWindow;
                case TaskType.PairMatch:
                    return _pairMatchWindow;
                default:
                    return _miniCaseWindow;
            }
        }

        private void OnTaskCompleted()
        {
            if (_currentWindow != null)
                _currentWindow.Hide();
            _winWindow.Initialize(GameplayManager.Instance.GetCorrectAnswersCount());
            _winWindow.Open();
        }

        private void OnTaskFailed()
        {
            if (_currentWindow != null)
                _currentWindow.Hide();
            _loseWindow.Open();
        }
    }
}
