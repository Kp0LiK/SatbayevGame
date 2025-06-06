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

            var taskType = GameplayManager.Instance.GetCurrentTaskType();
            var profession = GameplayManager.Instance.GetCurrentProfession();
            var levels = LevelSystem.Instance.GetLevelsFor(taskType, profession);
            if (levels == null)
            {
                Debug.LogWarning($"[SelectLevelWindowView] No levels found for task type: {taskType} and profession: {profession}");
                return;
            }

            Debug.Log($"[SelectLevelWindowView] Spawning {levels.Count} levels for task type: {taskType} and profession: {profession}");

            for (int i = 0; i < levels.Count; i++)
            {
                var newLevel = Instantiate(_levelPrefab, _content);

                // Первый уровень всегда открыт
                bool isOpen = (i == 0);

                if (!levels[i].IsLock)
                    isOpen = true;

                if (LevelSystem.Instance.IsLevelCompleted(taskType, profession, i))
                    isOpen = true;

                newLevel.Initialize(i, isOpen);
                _spawnedLevels.Add(newLevel);
            }
        }
    }
}