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
            _skipButton.onClick.AddListener(OnSkipButtonClick);
            GameplayManager.Instance.OnAttemptsChanged += OnAttemptsChanged;
        }

        protected virtual void OnDisable()
        {
            _submitButton.onClick.RemoveListener(OnSubmitButtonClick);
            _skipButton.onClick.RemoveListener(OnSkipButtonClick);
            if (GameplayManager.Instance != null)
            {
                GameplayManager.Instance.OnAttemptsChanged -= OnAttemptsChanged;
            }
        }

        public virtual void Initialize(ITaskData taskData)
        {
            _currentTask = taskData;
            _questionText.text = taskData.GetQuestionText();
            _isAnswerSelected = false;
            _submitButton.interactable = false;
            _skipButton.interactable = GameplayManager.Instance.HasMoreTasks();
            UpdateAttemptsText();
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
            Debug.Log(_isAnswerSelected);
            if (!_isAnswerSelected) return;

            var answer = GetSelectedAnswer();
            GameplayManager.Instance.SubmitAnswer(answer);
        }

        protected virtual void OnSkipButtonClick()
        {
            if (GameplayManager.Instance.HasMoreTasks())
            {
                GameplayManager.Instance.SkipTask();
            }
        }

        protected virtual void OnAttemptsChanged(int attempts)
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