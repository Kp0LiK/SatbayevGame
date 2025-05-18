using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class SelectLevelWindowView : BaseWindowView
    {
        [SerializeField] private Transform _content;
        [SerializeField] private LevelButtonIconView _levelPrefab;
        [SerializeField] private Button _leftArrow;
        [SerializeField] private Button _rightArrow;

        private List<LevelButtonIconView> _spawnedLevels;
        private TaskType _currentTaskType;

        private void Awake()
        {
            _spawnedLevels = new List<LevelButtonIconView>();
        }

        private void OnEnable()
        {
            SpawnLevels();
        }

        private void SpawnLevels()
        {
            // Clear previous levels
            foreach (var level in _spawnedLevels)
            {
                Destroy(level.gameObject);
            }
            _spawnedLevels.Clear();

            var levels = LevelSystem.Instance.GetLevelsFor(_currentTaskType);
            if (levels == null) return;

            for (int i = 0; i < levels.Count; i++)
            {
                var newLevel = Instantiate(_levelPrefab, _content);
                newLevel.Initialize(_currentTaskType, levels[i], i);
                _spawnedLevels.Add(newLevel);
            }
        }

        public void SetTaskType(TaskType taskType)
        {
            _currentTaskType = taskType;
            if (gameObject.activeInHierarchy)
            {
                SpawnLevels();
            }
        }
    }
}