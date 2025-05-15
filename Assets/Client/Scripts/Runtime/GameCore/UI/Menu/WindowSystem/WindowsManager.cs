using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class WindowsManager : MonoBehaviour
    {
        public static WindowsManager Instance { get; private set; }

        [SerializeField] private List<BaseWindowView> _windowViews;

        private BaseWindowView _currentWindowView;
        private BaseWindowView _previousWindowView;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _currentWindowView = _windowViews[0];
            _currentWindowView.OpenImmediately();
        }

        public T GetWindow<T>() where T : BaseWindowView
        {
            var window = _windowViews.FirstOrDefault(w => w is T);

            if (!ReferenceEquals(window, null)) return (T)window;
            Debug.LogError("[Windows Manager] Window hasn't initialized");
            if (_windowViews.Count <= 0) return null;
            return null;
        }

        private void OnResetButtonClick()
        {
            PlayerPrefs.DeleteAll();
        }

        public void OpenWindow<T>() where T : BaseWindowView
        {
            var window = _windowViews.FirstOrDefault(w => w is T);

            if (ReferenceEquals(window, null))
            {
                Debug.LogError("[Windows Manager] Window hasn't initialized " + typeof(T).Name);
                if (_windowViews.Count <= 0) return;

                _currentWindowView = _windowViews[0];
                _currentWindowView.Open();
                return;
            }

            if (!ReferenceEquals(_currentWindowView, null))
            {
                _previousWindowView = _currentWindowView;
                _currentWindowView.Close();
            }

            window.Open();
            _currentWindowView = window;
        }

        public void BackWindow()
        {
            if (ReferenceEquals(_previousWindowView, null)) return;
            _currentWindowView.Close();
            _currentWindowView = _windowViews[0];
            _currentWindowView.Open();
        }

        public void BackPreviewsWindow()
        {
            if (ReferenceEquals(_previousWindowView, null)) return;

            _currentWindowView.Close();
            _currentWindowView = _previousWindowView;
            _currentWindowView.Open();
        }

        public void CloseAlLWindows()
        {
            foreach (var panel in _windowViews) panel.CloseImmediately();
        }

#if UNITY_EDITOR
        [Button]
        private void InitAllWindows()
        {
            _windowViews = GetComponentsInChildren<BaseWindowView>(true).ToList();
        }
#endif
    }
}