using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public class MiniCaseWindowView : BaseGameplayWindowView
    {
        [SerializeField] private Transform _optionsContainer;
        [SerializeField] private Button _optionButtonPrefab;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _defaultColor;

        private int _selectedOptionIndex = -1;
        private Image[] _optionImages;

        public override void Initialize(ITaskData taskData)
        {
            base.Initialize(taskData);
            
            // Clear previous options
            foreach (Transform child in _optionsContainer)
            {
                Destroy(child.gameObject);
            }

            var miniCaseTask = (MiniCaseTaskData)taskData;
            _optionImages = new Image[miniCaseTask.answerOptions.Length];

            // Create option buttons
            for (int i = 0; i < miniCaseTask.answerOptions.Length; i++)
            {
                int index = i; // Capture for lambda
                var button = Instantiate(_optionButtonPrefab, _optionsContainer);
                button.GetComponentInChildren<TMP_Text>().text = miniCaseTask.answerOptions[i];
                button.onClick.AddListener(() => OnOptionSelected(index));
                
                var image = button.GetComponent<Image>();
                image.color = _defaultColor;
                _optionImages[i] = image;
            }

            _selectedOptionIndex = -1;
            _submitButton.interactable = false;
        }

        private void OnOptionSelected(int index)
        {
            // Reset previous selection color
            foreach (var optionImage in _optionImages)
            {
                optionImage.color = _defaultColor;
            }

            _selectedOptionIndex = index;
            _optionImages[index].color = _selectedColor;
            
            OnAnswerSelected(index);
        }

        protected override object GetSelectedAnswer()
        {
            return _selectedOptionIndex;
        }
    }
} 