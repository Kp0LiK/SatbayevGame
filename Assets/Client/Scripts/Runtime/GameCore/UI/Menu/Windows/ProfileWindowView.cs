using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public class ProfileWindowView : BaseWindowView
    {
        [SerializeField] private TMP_InputField _nameInputField;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button[] _professionButtons;
        
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _selectedColor;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClick);
            
            foreach (var button in _professionButtons)
            {
                button.onClick.AddListener(() => OnProfessionSelected(button));
            }

            GetCurrentInfo();
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClick);
            
            foreach (var button in _professionButtons)
            {
                button.onClick.RemoveListener(() => OnProfessionSelected(button));
            }
        }

        private void GetCurrentInfo()
        {
            _nameInputField.text = GameSession.Instance.GetPlayerName();
            _professionButtons[GameSession.Instance.SelectedProfessionIndex].image.color = _selectedColor;
        }

        private void OnBackButtonClick()
        {
            SaveInfo();
            WindowsManager.Instance.BackPreviewsWindow();
        }
        
        private void SaveInfo()
        {
            var playerName = _nameInputField.text;
            if (!string.IsNullOrEmpty(playerName))
            {
                GameSession.Instance.SavePlayerName(playerName);
                Debug.Log($"Player name saved: {playerName}");
            }
            else
            {
                Debug.LogWarning("Player name cannot be empty.");
            }
        }

        private void OnProfessionSelected(Button button)
        {
            foreach (var btn in _professionButtons)
            {
                btn.image.color = _defaultColor;
            }
            
            button.image.color = _selectedColor;
            int index = System.Array.IndexOf(_professionButtons, button);
            GameSession.Instance.SelectedProfessionIndex = index;
            GameSession.Instance.SelectProfession(index);
        }
    }
}