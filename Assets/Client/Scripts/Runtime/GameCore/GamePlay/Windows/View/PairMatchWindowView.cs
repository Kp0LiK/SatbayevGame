using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

namespace Client
{
    public class PairMatchWindowView : BaseGameplayWindowView
    {
        [Header("Pair Match Settings")]
        [SerializeField] private Transform _leftColumn;
        [SerializeField] private Transform _rightColumn;
        [SerializeField] private PairMatchItem _itemPrefab;
        [SerializeField] private LineRenderer _linePrefab;
        [SerializeField] private RectTransform _lineCanvas;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private float _timeLimit = 30f;
        [SerializeField] private Color _correctLineColor;
        [SerializeField] private Color _incorrectLineColor;

        private List<PairMatchItem> _leftItems = new();
        private List<PairMatchItem> _rightItems = new();
        private List<LineRenderer> _lines = new();

        private PairMatchItem _selectedLeft;
        private PairMatchTaskData _taskData;
        private Dictionary<string, string> _matches = new();
        private Dictionary<LineRenderer, bool> _lineResults = new();

        private float _timer;
        private bool _timerRunning;
        private bool _isSubmitted;

        private class PairConnection
        {
            public PairMatchItem left;
            public PairMatchItem right;
            public LineRenderer line;
            public bool isCorrect;
        }
        private List<PairConnection> _connections = new();

        public class PairMatchResult
        {
            public Dictionary<string, string> UserPairs;
            public int CorrectCount;
            public bool IsWin;
        }

        private void Update()
        {
            if (!_timerRunning) return;
            _timer -= Time.deltaTime;
            _timerText.text = Mathf.CeilToInt(_timer).ToString();

            if (_timer <= 0)
            {
                _timerRunning = false;
                GameplayManager.Instance.SkipTask();
            }
        }

        protected override void ResetState()
        {
            base.ResetState();
            _selectedLeft = null;
            _matches.Clear();
            _lineResults.Clear();
            _connections.Clear();
            _timer = _timeLimit;
            _timerRunning = false;
            _timerText.text = "";

            foreach (Transform child in _leftColumn)
                Destroy(child.gameObject);
            foreach (Transform child in _rightColumn)
                Destroy(child.gameObject);
            foreach (var line in _lines)
                Destroy(line.gameObject);

            _leftItems.Clear();
            _rightItems.Clear();
            _lines.Clear();
            _submitButton.interactable = false;
        }

        public override void Initialize(ITaskData taskData)
        {
            base.Initialize(taskData);
            _isSubmitted = false;
            _taskData = (PairMatchTaskData)taskData;
            _taskData.Initialize();
            _questionText.text = taskData.GetQuestionText();

            for (int i = 0; i < _taskData.leftColumn.Length; i++)
            {
                var left = Instantiate(_itemPrefab, _leftColumn);
                left.Initialize(_taskData.leftColumn[i], () => OnLeftItemClicked(left));
                _leftItems.Add(left);

                var right = Instantiate(_itemPrefab, _rightColumn);
                right.Initialize(_taskData.rightColumn[i], () => OnRightItemClicked(right));
                _rightItems.Add(right);
            }

            _timerRunning = true;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _submitButton.onClick.AddListener(OnSubmit);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _submitButton.onClick.RemoveListener(OnSubmit);
        }

        private void OnLeftItemClicked(PairMatchItem item)
        {
            if (_isSubmitted || item.IsMatched) return;
            if (_selectedLeft != null)
            {
                _selectedLeft.SetSelected(false);
            }
            _selectedLeft = item;
            item.SetSelected(true);
        }

        private void OnRightItemClicked(PairMatchItem right)
        {
            if (_isSubmitted || _selectedLeft == null || right.IsMatched) return;

            _matches[_selectedLeft.Text] = right.Text;
            _selectedLeft.SetMatched(true);
            right.SetMatched(true);

            bool isCorrect = _taskData.correctPairs[_selectedLeft.Text] == right.Text;
            _connections.Add(new PairConnection
            {
                left = _selectedLeft,
                right = right,
                line = null, // линия будет нарисована после Submit
                isCorrect = isCorrect
            });

            _selectedLeft = null;

            if (_matches.Count >= _taskData.leftColumn.Length)
            {
                _submitButton.interactable = true;
            }
        }

        private async void OnSubmit()
        {
            _isSubmitted = true;
            _timerRunning = false;
            int correctCount = 0;
            foreach (var conn in _connections)
            {
                conn.line = DrawConnectionLine(conn.left.RectTransform, conn.right.RectTransform, conn.isCorrect ? _correctLineColor : _incorrectLineColor);
                conn.left.SetResultColor(conn.isCorrect ? _correctLineColor : _incorrectLineColor);
                conn.right.SetResultColor(conn.isCorrect ? _correctLineColor : _incorrectLineColor);
                if (conn.isCorrect) correctCount++;
            }
            int total = _connections.Count;
            bool isWin = correctCount > total / 2;
            Debug.Log($"PairMatch: correctCount={correctCount}, total={total}, isWin={isWin}");
            await Awaiter(3f);
            if (isWin)
                GameplayManager.Instance.SuccessCompleteLevel(correctCount);
            else
                GameplayManager.Instance.LoseLevel();
        }

        private Task Awaiter(float seconds)
        {
            return Task.Delay((int)(seconds * 1000));
        }

        private LineRenderer DrawConnectionLine(RectTransform from, RectTransform to, Color color)
        {
            var line = Instantiate(_linePrefab, _lineCanvas);
            Vector3[] positions = new Vector3[2]
            {
                from.position,
                to.position
            };
            line.positionCount = 2;
            line.SetPositions(positions);
            line.startColor = line.endColor = color;
            _lines.Add(line);
            return line;
        }

        protected override object GetSelectedAnswer() => _matches;
    }
}