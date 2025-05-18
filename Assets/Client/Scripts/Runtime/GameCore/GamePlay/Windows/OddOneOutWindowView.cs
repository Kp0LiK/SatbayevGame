using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public class OddOneOutWindowView : BaseGameplayWindowView
    {
        [SerializeField] private Transform _optionsContainer;
        [SerializeField] private Button _optionButtonPrefab;

        private OddOneOutTaskData _currentTask;
        private int _selectedOptionIndex = -1;

        public override void Initialize(ITaskData taskData)
        {
            base.Initialize(taskData);
            _currentTask = (OddOneOutTaskData)taskData;
            
            // Clear previous options
            foreach (Transform child in _optionsContainer)
            {
                Destroy(child.gameObject);
            }

            // Create option buttons
            for (int i = 0; i < _currentTask.options.Length; i++)
            {
                int index = i; // Capture for lambda
                var button = Instantiate(_optionButtonPrefab, _optionsContainer);
                button.GetComponentInChildren<TMP_Text>().text = _currentTask.options[i];
                button.onClick.AddListener(() => OnOptionSelected(index));
            }

            _selectedOptionIndex = -1;
            _submitButton.interactable = false;
        }

        private void OnOptionSelected(int index)
        {
            _selectedOptionIndex = index;
            _submitButton.interactable = true;

            // Update visual selection state
            for (int i = 0; i < _optionsContainer.childCount; i++)
            {
                var button = _optionsContainer.GetChild(i).GetComponent<Button>();
                var colors = button.colors;
                colors.normalColor = i == index ? Color.yellow : Color.white;
                button.colors = colors;
            }
        }

        protected override bool ValidateAnswer()
        {
            return _selectedOptionIndex == _currentTask.oddIndex;
        }

        protected override int CalculateScore()
        {
            return 100; // Base score for correct answer
        }
    }
} 