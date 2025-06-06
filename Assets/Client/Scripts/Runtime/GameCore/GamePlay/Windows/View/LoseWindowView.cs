using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Client
{
    public class LoseWindowView : BaseWindowView
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _backToMenuButton;

        private void OnEnable()
        {
            _retryButton.onClick.AddListener(OnRetryButtonClick);
            _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);
        }

        private void OnDisable()
        {
            _retryButton.onClick.RemoveListener(OnRetryButtonClick);
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClick);
        }

        private void OnRetryButtonClick()
        {
            var taskType = GameplayManager.Instance.GetCurrentTaskType();
            var profession = GameplayManager.Instance.GetCurrentProfession();
            var currentLevel = GameplayManager.Instance.GetCurrentLevelIndex();
            GameplayManager.Instance.StartTask(taskType, profession, currentLevel);
            Close();
        }

        private void OnBackToMenuButtonClick()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}