using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Client
{
    public class OddOneOutItem : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _image;
        [SerializeField] private Image _highlight;
        [SerializeField] private Color _correctColor;
        [SerializeField] private Color _wrongColor;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _defaultColor;

        private int _index;
        private event Action<int> Clicked;
        private Image _background;
        private bool _isSelected;

        private void Awake()
        {
            _background = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Setup(TaskOption option, int index, Action<int> onClick)
        {
            _index = index;
            Clicked = onClick;

            // Текст
            if (!string.IsNullOrEmpty(option.text))
            {
                _text.text = option.text;
                _text.gameObject.SetActive(true);
            }
            else
            {
                _text.gameObject.SetActive(false);
            }

            // Картинка
            if (option.sprite != null)
            {
                _image.sprite = option.sprite;
                _image.gameObject.SetActive(true);
            }
            else
            {
                _image.gameObject.SetActive(false);
            }

            _highlight.gameObject.SetActive(false);
            _button.interactable = true;
            Deselect();
        }

        public void Select()
        {
            _isSelected = true;
            if (_background != null)
                _background.color = _selectedColor;
        }

        public void Deselect()
        {
            _isSelected = false;
            if (_background != null)
                _background.color = _defaultColor;
        }

        public void SetResult(bool isCorrect)
        {
            _highlight.gameObject.SetActive(true);
            _highlight.color = isCorrect ? _correctColor : _wrongColor;
        }

        public void SetInteractable(bool value)
        {
            _button.interactable = value;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnButtonClick()
        {
            if (!_isSelected)
                Select();
            Clicked?.Invoke(_index);
        }
    }
}