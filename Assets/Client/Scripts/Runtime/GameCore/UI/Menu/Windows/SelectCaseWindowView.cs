using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class SelectCaseWindowView : BaseWindowView
    {
        [SerializeField] private Button _itCaseButton;

        private void OnEnable()
        {
            _itCaseButton.onClick.AddListener(OnItCaseButtonClick);
        }

        private void OnItCaseButtonClick()
        {
            WindowsManager.Instance.OpenWindow<SelectLevelWindowView>();
        }
    }
}