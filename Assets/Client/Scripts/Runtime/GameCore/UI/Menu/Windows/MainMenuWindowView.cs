using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class MainMenuWindowView : BaseWindowView
    {
        [SerializeField] private Button _changeProfileButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Button _settingButton;
        
        [SerializeField] private TMP_Text _selectedProfessionLabel;
        
        private Profession _selectedProfession;

        private void Awake()
        {
            _selectedProfession = (Profession)GameSession.Instance.SelectedProfessionIndex;
            _selectedProfessionLabel.text = $"Your profession: {_selectedProfession.ToString()}";
            GameplayManager.Instance.SetCurrentProfession(_selectedProfession);
        }

        private void OnEnable()
        {
            _changeProfileButton.onClick.AddListener(OnChangeProfileButtonClick);
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _leaderboardButton.onClick.AddListener(OnLeaderboardButtonClick);
            _settingButton.onClick.AddListener(OnSettingButtonClick);
            
            GameSession.Instance.ProfessionSelected += OnProfessionSelected;
        }



        private void OnDisable()
        {
            _changeProfileButton.onClick.RemoveListener(OnChangeProfileButtonClick);
            _playButton.onClick.RemoveListener(OnPlayButtonClick);
            _leaderboardButton.onClick.RemoveListener(OnLeaderboardButtonClick);
            _settingButton.onClick.RemoveListener(OnSettingButtonClick);
            
            GameSession.Instance.ProfessionSelected += OnProfessionSelected;
        }
        
        private void OnProfessionSelected(int value)
        {
            _selectedProfession = (Profession)value;
            _selectedProfessionLabel.text = $"Your profession: {_selectedProfession.ToString()}";
            GameplayManager.Instance.SetCurrentProfession(_selectedProfession);
        }

        private void OnChangeProfileButtonClick()
        {
            WindowsManager.Instance.OpenWindow<ProfileWindowView>();
        }

        private void OnPlayButtonClick()
        {
            WindowsManager.Instance.OpenWindow<SelectCaseWindowView>();
        }

        private void OnLeaderboardButtonClick()
        {
            WindowsManager.Instance.OpenWindow<LeaderboardWindowView>();
        }

        private void OnSettingButtonClick()
        {
            WindowsManager.Instance.OpenWindow<SettingsWindowView>();
        }
    }
}