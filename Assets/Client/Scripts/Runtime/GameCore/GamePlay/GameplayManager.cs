using UnityEngine;
using System;

namespace Client
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance { get; private set; }

        [SerializeField] private TaskSet _currentTaskSet;

        public event Action<TaskType, int> OnLevelStarted;
        public event Action<int> OnLevelCompleted;
        public event Action OnLevelFailed;

        private TaskType _currentTaskType;
        private int _currentLevelIndex;
        private ITaskData _currentTask;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void StartLevel(TaskType taskType, int levelIndex)
        {
            _currentTaskType = taskType;
            _currentLevelIndex = levelIndex;
            _currentTask = GetTaskForLevel(taskType, levelIndex);
            
            OnLevelStarted?.Invoke(taskType, levelIndex);
        }

        public void CompleteLevel(int score)
        {
            LevelSystem.Instance.CompleteLevel(_currentTaskType, _currentLevelIndex, score);
            OnLevelCompleted?.Invoke(score);
        }

        public void FailLevel()
        {
            OnLevelFailed?.Invoke();
        }

        public ITaskData GetCurrentTask()
        {
            return _currentTask;
        }

        private ITaskData GetTaskForLevel(TaskType taskType, int levelIndex)
        {
            switch (taskType)
            {
                case TaskType.MiniCase:
                    return _currentTaskSet.miniCaseTasks[levelIndex];
                case TaskType.OddOneOut:
                    return _currentTaskSet.oddOneOutTasks[levelIndex];
                case TaskType.PairMatch:
                    return _currentTaskSet.pairMatchTasks[levelIndex];
                case TaskType.Sequence:
                    return _currentTaskSet.sequenceTasks[levelIndex];
                default:
                    throw new ArgumentException($"Unknown task type: {taskType}");
            }
        }
    }
} 