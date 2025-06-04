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
            
            // Проверяем наличие следующего уровня
            var levels = LevelSystem.Instance.GetLevelsFor(TaskType.MiniCase);
            bool hasNextLevel = levels != null && GameplayManager.Instance.GetCurrentLevelIndex() + 1 < levels.Count;
            _nextLevelButton.gameObject.SetActive(hasNextLevel);
        }

        private void OnNextLevelButtonClick()
        {
            GameplayManager.Instance.StartTask(TaskType.MiniCase, GameplayManager.Instance.GetCurrentLevelIndex() + 1);
        }

        private void OnMenuButtonClick()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}