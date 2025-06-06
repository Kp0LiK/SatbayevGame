using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class PairMatchItem : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _background;
        [SerializeField] private Color _selectedColor = Color.yellow;
        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _matchedColor = Color.green;

        private Action _onClick;
        private bool _isSelected;
        private bool _isMatched;

        public string Text { get; private set; }
        public RectTransform RectTransform => (RectTransform)transform;
        public bool IsMatched => _isMatched;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        public void Initialize(string text, Action onClick)
        {
            Text = text;
            _text.text = text;
            _onClick = onClick;
            ResetState();
        }

        public void ResetState()
        {
            _isMatched = false;
            _isSelected = false;
            _button.interactable = true;
            _background.color = _defaultColor;
        }

        public void SetSelected(bool selected)
        {
            if (_isMatched) return;
            
            _isSelected = selected;
            _background.color = selected ? _selectedColor : _defaultColor;
        }

        public void SetMatched(bool matched)
        {
            _isMatched = matched;
            _button.interactable = false;
            _background.color = _matchedColor;
        }

        public void SetResultColor(Color color)
        {
            _background.color = color;
        }

        private void OnClick()
        {
            if (!_isMatched)
            {
                _onClick?.Invoke();
            }
        }
    }
}