using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class SelectCaseWindowView : BaseWindowView
    {
        [SerializeField] private Button _miniCaseButton;
        [SerializeField] private Button _oddOneOutButton;
        [SerializeField] private Button _pairMatchButton;
        [SerializeField] private Button _sequence;

        private void OnEnable()
        {
            _miniCaseButton.onClick.AddListener(OnButtonClick);
            _oddOneOutButton.onClick.AddListener(OnOddOneOutButtonClick);
            _pairMatchButton.onClick.AddListener(OnPairMatchButtonClick);
            _sequence.onClick.AddListener(OnSequenceButtonClick);
            
            BackButton.onClick.AddListener(OnBackButtonClick);
        }

        private void OnDisable()
        {
            _miniCaseButton.onClick.RemoveListener(OnButtonClick);
            _oddOneOutButton.onClick.RemoveListener(OnOddOneOutButtonClick);
            _pairMatchButton.onClick.RemoveListener(OnPairMatchButtonClick);
            _sequence.onClick.RemoveListener(OnSequenceButtonClick);
            
            BackButton.onClick.RemoveListener(OnBackButtonClick);
        }

        private void OnButtonClick()
        {
            GameplayManager.Instance.SetCurrentTaskType(TaskType.MiniCase);
            WindowsManager.Instance.OpenWindow<SelectLevelWindowView>();
        }

        private void OnOddOneOutButtonClick()
        {
            GameplayManager.Instance.SetCurrentTaskType(TaskType.OddOneOut);
            WindowsManager.Instance.OpenWindow<SelectLevelWindowView>();
        }

        private void OnPairMatchButtonClick()
        {
            GameplayManager.Instance.SetCurrentTaskType(TaskType.PairMatch);
            WindowsManager.Instance.OpenWindow<SelectLevelWindowView>();
        }

        private void OnSequenceButtonClick()
        {
            GameplayManager.Instance.SetCurrentTaskType(TaskType.Sequence);
            WindowsManager.Instance.OpenWindow<SelectLevelWindowView>();
        }

        private void OnBackButtonClick()
        {
            WindowsManager.Instance.BackWindow();
        }
    }
}