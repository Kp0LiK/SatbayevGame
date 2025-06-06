using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public abstract class BaseGameplayWindowView : BaseWindowView, ITaskView
    {
        [SerializeField] protected TMP_Text _questionText;
        [SerializeField] protected Button _submitButton;
        [SerializeField] protected Button _skipButton;
        //[SerializeField] protected TMP_Text _attemptsText;

        protected ITaskData _currentTask;
        protected bool _isAnswerSelected;

        protected virtual void OnEnable()
        {
            _submitButton.onClick.AddListener(OnSubmitButtonClick);
            if (_skipButton != null)
            {
                _skipButton.onClick.AddListener(OnSkipButtonClick);
            }

            GameplayManager.Instance.OnAttemptsChanged += OnAttemptsChanged;
        }

        protected virtual void OnDisable()
        {
            _submitButton.onClick.RemoveListener(OnSubmitButtonClick);
            if (_skipButton != null)
            {
                _skipButton.onClick.RemoveListener(OnSkipButtonClick);
            }

            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.OnAttemptsChanged -= OnAttemptsChanged;
            }
        }

        public virtual void Initialize(ITaskData taskData)
        {
            ResetState();
            _currentTask = taskData;
            _questionText.text = taskData.GetQuestionText();
            _isAnswerSelected = false;
            _submitButton.interactable = false;
            if (_skipButton != null)
            {
                _skipButton.interactable = GameplayManager.Instance.HasMoreTasks();
            }

            UpdateAttemptsText();
        }

        protected virtual void ResetState()
        {
            _currentTask = null;
            _isAnswerSelected = false;
            _submitButton.interactable = false;
            if (_skipButton != null)
            {
                _skipButton.interactable = true;
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void OnAnswerSelected(object answer)
        {
            _isAnswerSelected = true;
            _submitButton.interactable = true;
        }

        private void OnSubmitButtonClick()
        {
            if (!_isAnswerSelected) return;

            var answer = GetSelectedAnswer();
            GameplayManager.Instance.SubmitAnswer(answer);
        }

        private void OnSkipButtonClick()
        {
            if (GameplayManager.Instance.HasMoreTasks())
            {
                GameplayManager.Instance.SkipTask();
            }
        }

        private void OnAttemptsChanged(int attempts)
        {
            UpdateAttemptsText();
        }

        private void UpdateAttemptsText()
        {
            /*if (_attemptsText != null)
            {
                _attemptsText.text = $"Attempts: {GameplayManager.Instance.GetRemainingAttempts()}";
            }*/
        }

        protected abstract object GetSelectedAnswer();
    }
}