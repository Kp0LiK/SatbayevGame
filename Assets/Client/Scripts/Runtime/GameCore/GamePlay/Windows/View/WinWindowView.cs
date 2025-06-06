using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Client
{
    public class WinWindowView : BaseWindowView
    {
        [SerializeField] private TMP_Text _correctAnswersText;
        [SerializeField] private Button _nextLevelButton;

        private void OnEnable()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
            BackButton.onClick.AddListener(OnMenuButtonClick);
        }

        private void OnDisable()
        {
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClick);
            BackButton.onClick.RemoveListener(OnMenuButtonClick);
        }

        public void Initialize(int correctAnswers)
        {
            _correctAnswersText.text = $"Correct Answers: {correctAnswers}";
            
            var taskType = GameplayManager.Instance.GetCurrentTaskType();
            var profession = GameplayManager.Instance.GetCurrentProfession();
            var levels = LevelSystem.Instance.GetLevelsFor(taskType, profession);
            bool hasNextLevel = levels != null && GameplayManager.Instance.GetCurrentLevelIndex() + 1 < levels.Count;
            _nextLevelButton.gameObject.SetActive(hasNextLevel);
        }

        private void OnNextLevelButtonClick()
        {
            var taskType = GameplayManager.Instance.GetCurrentTaskType();
            var profession = GameplayManager.Instance.GetCurrentProfession();
            GameplayManager.Instance.StartTask(taskType, profession, GameplayManager.Instance.GetCurrentLevelIndex() + 1);
        }

        private void OnMenuButtonClick()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}