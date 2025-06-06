using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class PairMatchTaskData : ITaskData
    {
        [Serializable]
        public class PairData
        {
            public string leftText;
            public string rightText;
            //public string description; // Опциональное описание пары
        }

        [Header("Task Settings")]
        [SerializeField] private string _questionText = "Соедини пары:";
        [SerializeField] private List<PairData> _pairs = new();
        [SerializeField] private bool _shuffleRightColumn = true;
        [SerializeField] private bool _shuffleLeftColumn = false;

        private string[] _leftColumn;
        private string[] _rightColumn;
        private Dictionary<string, string> _correctPairs;

        public string[] leftColumn => _leftColumn;
        public string[] rightColumn => _rightColumn;
        public Dictionary<string, string> correctPairs => _correctPairs;

        public string GetQuestionText() => _questionText;

        public TaskType GetTaskType() => TaskType.PairMatch;

        public void Initialize()
        {
            if (_pairs == null || _pairs.Count == 0)
            {
                Debug.LogError("PairMatchTaskData: No pairs defined!");
                return;
            }

            // Инициализируем массивы
            _leftColumn = new string[_pairs.Count];
            _rightColumn = new string[_pairs.Count];
            _correctPairs = new Dictionary<string, string>();

            // Заполняем массивы и создаем правильные пары
            for (int i = 0; i < _pairs.Count; i++)
            {
                _leftColumn[i] = _pairs[i].leftText;
                _rightColumn[i] = _pairs[i].rightText;
                _correctPairs[_pairs[i].leftText] = _pairs[i].rightText;
            }

            // Перемешиваем колонки если нужно
            if (_shuffleLeftColumn)
                ShuffleColumn(ref _leftColumn);
            
            if (_shuffleRightColumn)
                ShuffleColumn(ref _rightColumn);
        }

        private void ShuffleColumn(ref string[] column)
        {
            var tempArray = column.ToArray();
            
            for (var i = tempArray.Length - 1; i > 0; i--)
            {
                var randomIndex = UnityEngine.Random.Range(0, i + 1);
                (tempArray[i], tempArray[randomIndex]) = (tempArray[randomIndex], tempArray[i]);
            }

            column = tempArray;
        }

        public bool ValidateAnswer(object answer)
        {
            if (answer is not Dictionary<string, string> userPairs)
                return false;

            if (userPairs.Count != _correctPairs.Count)
                return false;

            foreach (var pair in _correctPairs)
            {
                if (!userPairs.TryGetValue(pair.Key, out var match) || match != pair.Value)
                    return false;
            }

            return true;
        }

        public int CountCorrectPairs(Dictionary<string, string> userPairs)
        {
            int count = 0;
            foreach (var pair in _correctPairs)
            {
                if (userPairs.TryGetValue(pair.Key, out var match) && match == pair.Value)
                    count++;
            }
            return count;
        }
    }
}