using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Client
{
    public abstract class BaseWindowView : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private WindowAnimationType _animationType = WindowAnimationType.Fade;

        public event Action<BaseWindowView> WindowOpened;
        public event Action<BaseWindowView> WindowClosed;
        public event Action BackButtonPressed;

        protected Button BackButton => _backButton;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_animationType == WindowAnimationType.Fade)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
        }

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
            AnimateOpen();
            WindowOpened?.Invoke(this);
        }

        public void Close()
        {
            AnimateClose(() => {
                WindowClosed?.Invoke(this);
                gameObject.SetActive(false);
            });
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

        private void AnimateOpen()
        {
            var preset = WindowsManager.Instance.AnimationSettings.GetPreset(_animationType);
            
            switch (_animationType)
            {
                case WindowAnimationType.Fade:
                    if (_canvasGroup != null)
                    {
                        _canvasGroup.alpha = 0f;
                        _canvasGroup.DOFade(1f, preset.duration)
                            .SetEase(preset.openEase);
                    }
                    break;

                case WindowAnimationType.Scale:
                    if (_rectTransform != null)
                    {
                        _rectTransform.localScale = Vector3.zero;
                        _rectTransform.DOScale(Vector3.one, preset.duration)
                            .SetEase(preset.openEase);
                    }
                    break;

                case WindowAnimationType.ScaleUp:
                    if (_rectTransform != null)
                    {
                        _rectTransform.localScale = Vector3.one;
                        _rectTransform.DOScale(Vector3.one * preset.scaleMultiplier, preset.duration)
                            .SetEase(preset.openEase)
                            .OnComplete(() => _rectTransform.DOScale(Vector3.one, preset.duration * 0.5f)
                                .SetEase(preset.openEase));
                    }
                    break;
            }
        }

        private void AnimateClose(Action onComplete)
        {
            var preset = WindowsManager.Instance.AnimationSettings.GetPreset(_animationType);
            
            switch (_animationType)
            {
                case WindowAnimationType.Fade:
                    if (_canvasGroup != null)
                    {
                        _canvasGroup.DOFade(0f, preset.duration)
                            .SetEase(preset.closeEase)
                            .OnComplete(() => onComplete?.Invoke());
                    }
                    else
                    {
                        onComplete?.Invoke();
                    }
                    break;

                case WindowAnimationType.Scale:
                    if (_rectTransform != null)
                    {
                        _rectTransform.DOScale(Vector3.zero, preset.duration)
                            .SetEase(preset.closeEase)
                            .OnComplete(() => onComplete?.Invoke());
                    }
                    else
                    {
                        onComplete?.Invoke();
                    }
                    break;

                case WindowAnimationType.ScaleUp:
                    if (_rectTransform != null)
                    {
                        _rectTransform.DOScale(Vector3.one * preset.scaleMultiplier, preset.duration * 0.5f)
                            .SetEase(preset.closeEase)
                            .OnComplete(() => onComplete?.Invoke());
                    }
                    else
                    {
                        onComplete?.Invoke();
                    }
                    break;

                default:
                    onComplete?.Invoke();
                    break;
            }
        }
    }

    public enum WindowAnimationType
    {
        None,
        Fade,
        Scale,
        ScaleUp
    }
}