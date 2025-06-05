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

            var levels = LevelSystem.Instance.GetLevelsFor(GameplayManager.Instance.GetCurrentTaskType());
            if (levels == null)
            {
                Debug.LogWarning($"[SelectLevelWindowView] No levels found for task type: {GameplayManager.Instance.GetCurrentTaskType()}");
                return;
            }

            Debug.Log($"[SelectLevelWindowView] Spawning {levels.Count} levels for task type: {GameplayManager.Instance.GetCurrentTaskType()}");

            for (int i = 0; i < levels.Count; i++)
            {
                var newLevel = Instantiate(_levelPrefab, _content);

                // Первый уровень всегда открыт
                bool isOpen = (i == 0);

                if (!levels[i].IsLock)
                    isOpen = true;

                if (ProgressManager.Instance.IsLevelCompleted(GameplayManager.Instance.GetCurrentTaskType(), i))
                    isOpen = true;

                newLevel.Initialize(i, isOpen);
                _spawnedLevels.Add(newLevel);
            }
        }
    }
}