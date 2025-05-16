using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class SelectLevelWindowView : BaseWindowView
    {
        [SerializeField] private Transform _content;
        [SerializeField] private LevelButtonIconView _levelPrefab;
        
        [SerializeField] private LevelSystem _levelManager;

        [SerializeField] private Button _leftArrow;
        [SerializeField] private Button _rightArrow;

        private List<LevelButtonIconView> _spawnedLevels;
        private TaskType _currentTaskType;

        private void Awake()
        {
            _spawnedLevels = new List<LevelButtonIconView>();
            SpawnLevels();
        }

        private void SpawnLevels()
        {
            var index = 0;
            var levels = _levelManager.GetLevelsFor(_currentTaskType);
            
            foreach (var level in levels)
            {
                var newLevel = Instantiate(_levelPrefab, _content);
                newLevel.UpdateLevel(level, index);
                index++;
                _spawnedLevels.Add(newLevel);
            }
        }
    }
}