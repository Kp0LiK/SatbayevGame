namespace Client
{
    public class SettingsWindowView : BaseWindowView
    {
        private void OnEnable()
        {
            BackButton.onClick.AddListener(OnBackButtonClick);
        }

        private void OnDisable()
        {
            BackButton.onClick.RemoveListener(OnBackButtonClick);
        }

        private void OnBackButtonClick()
        {
            WindowsManager.Instance.BackPreviewsWindow();
        }
    }
}