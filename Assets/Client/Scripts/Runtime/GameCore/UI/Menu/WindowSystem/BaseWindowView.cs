using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public abstract class BaseWindowView : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        public event Action<BaseWindowView> WindowOpened;
        public event Action<BaseWindowView> WindowClosed;

        public event Action BackButtonPressed;

        protected Button BackButton => _backButton;

        private void OnEnable()
        {
            if (ReferenceEquals(_backButton, null))
                return;

            _backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            if (ReferenceEquals(_backButton, null))
                return;

            _backButton.onClick.RemoveListener(Back);
        }

        public void Open()
        {
            gameObject.SetActive(true);

            WindowOpened?.Invoke(this);
        }

        public void Close()
        {
            WindowClosed?.Invoke(this);
            gameObject.SetActive(false);
        }

        private void Back()
        {
            WindowsManager.Instance.BackWindow();
            BackButtonPressed?.Invoke();
        }

        public void OpenImmediately()
        {
            gameObject.SetActive(true);

            WindowOpened?.Invoke(this);
        }

        public void CloseImmediately()
        {

            WindowClosed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}