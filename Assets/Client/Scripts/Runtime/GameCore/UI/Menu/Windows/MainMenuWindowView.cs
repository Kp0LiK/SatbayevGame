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

        private void OnEnable()
        {
            _changeProfileButton.onClick.AddListener(OnChangeProfileButtonClick);
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _leaderboardButton.onClick.AddListener(OnLeaderboardButtonClick);
            _settingButton.onClick.AddListener(OnSettingButtonClick);
        }

        private void OnDisable()
        {
            _changeProfileButton.onClick.RemoveListener(OnChangeProfileButtonClick);
            _playButton.onClick.RemoveListener(OnPlayButtonClick);
            _leaderboardButton.onClick.RemoveListener(OnLeaderboardButtonClick);
            _settingButton.onClick.RemoveListener(OnSettingButtonClick);
        }

        private void OnChangeProfileButtonClick()
        {
            WindowsManager.Instance.OpenWindow<ProfileWindowView>();
        }

        private void OnPlayButtonClick()
        {
            //WindowsManager.Instance.OpenWindow<>();
            Debug.Log("Click Play button");
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