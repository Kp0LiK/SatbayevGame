using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Client
{
    public class OddOneOutWindowView : BaseGameplayWindowView
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private OddOneOutItem _itemPrefab;
        [SerializeField] private Button _hintButton;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private float _timePerTask = 20f;

        private OddOneOutTaskData _currentTask;
        private List<OddOneOutItem> _items = new();
        private int _selectedIndex = -1;
        private float _timer;
        private bool _timerActive;
        private bool _hintUsed;
        private int _hiddenIndex = -1;

        protected override void OnEnable()
        {
            base.OnEnable();
            _hintButton.onClick.AddListener(OnHintClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _hintButton.onClick.RemoveListener(OnHintClicked);
        }

        private void Update()
        {
            if (_timerActive)
            {
                _timer -= Time.deltaTime;
                _timerText.text = Mathf.CeilToInt(_timer).ToString();
                if (_timer <= 0f)
                {
                    _timerActive = false;
                    OnTimeOut();
                }
            }
        }

        public override void Initialize(ITaskData taskData)
        {
            base.Initialize(taskData);
            _currentTask = (OddOneOutTaskData)taskData;
            _timer = _timePerTask;
            _timerActive = true;
            _hintUsed = false;
            _hintButton.interactable = true;
            _hiddenIndex = -1;
            _questionText.text = taskData.GetQuestionText();

            foreach (var item in _items)
                Destroy(item.gameObject);
            _items.Clear();

            for (int i = 0; i < _currentTask.options.Count; i++)
            {
                int index = i;
                var item = Instantiate(_itemPrefab, _itemsContainer);
                item.Setup(_currentTask.options[i], index, OnItemSelected);
                _items.Add(item);
                item.transform.localScale = Vector3.zero;
                item.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack).SetDelay(i * 0.05f);
            }

            _selectedIndex = -1;
            _submitButton.interactable = false;
        }

        private void OnItemSelected(int index)
        {
            if (_selectedIndex == index || !_timerActive || index == _hiddenIndex) return;
            if (_selectedIndex != -1)
            {
                _items[_selectedIndex].Deselect();
                _items[_selectedIndex].SetInteractable(true);
            }
            _selectedIndex = index;
            _items[index].Select();
            _items[index].SetInteractable(false);
            OnAnswerSelected(index);
        }

        protected override object GetSelectedAnswer() => _selectedIndex;

        private void OnTimeOut()
        {
            GameplayManager.Instance.SkipTask();
        }

        private void OnHintClicked()
        {
            if (_hintUsed) return;
            _hintUsed = true;
            _hintButton.interactable = false;
            List<int> wrongIndexes = new();
            for (int i = 0; i < _currentTask.options.Count; i++)
                if (i != _currentTask.oddIndex)
                    wrongIndexes.Add(i);
            if (wrongIndexes.Count > 0)
            {
                _hiddenIndex = wrongIndexes[Random.Range(0, wrongIndexes.Count)];
                _items[_hiddenIndex].Hide();
            }
        }
    }
}