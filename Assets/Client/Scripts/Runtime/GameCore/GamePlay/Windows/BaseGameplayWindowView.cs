using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public abstract class BaseGameplayWindowView : BaseWindowView
    {
        [SerializeField] protected TMP_Text _questionText;
        [SerializeField] protected Button _submitButton;
        [SerializeField] protected Button _skipButton;

        protected virtual void OnEnable()
        {
            _submitButton.onClick.AddListener(OnSubmitButtonClick);
            _skipButton.onClick.AddListener(OnSkipButtonClick);
        }

        protected virtual void OnDisable()
        {
            _submitButton.onClick.RemoveListener(OnSubmitButtonClick);
            _skipButton.onClick.RemoveListener(OnSkipButtonClick);
        }

        public virtual void Initialize(ITaskData taskData)
        {
            _questionText.text = taskData.GetQuestionText();
        }

        protected virtual void OnSubmitButtonClick()
        {
            if (ValidateAnswer())
            {
                int score = CalculateScore();
                GameplayManager.Instance.CompleteLevel(score);
            }
            else
            {
                GameplayManager.Instance.FailLevel();
            }
        }

        protected virtual void OnSkipButtonClick()
        {
            GameplayManager.Instance.FailLevel();
        }

        protected abstract bool ValidateAnswer();
        protected abstract int CalculateScore();
    }
} 