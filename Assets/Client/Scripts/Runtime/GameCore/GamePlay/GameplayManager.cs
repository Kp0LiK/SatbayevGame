using UnityEngine;
using System;

namespace Client
{
    public class GameplayManager : MonoBehaviour, ITaskManager
    {
        public static GameplayManager Instance { get; private set; }

        private const int MAX_ATTEMPTS = 3;

        public event Action<ITaskData> OnTaskLoaded;
        public event Action<bool> OnAnswerSubmitted;
        public event Action OnTaskCompleted;
        public event Action OnTaskFailed;
        public event Action<int> OnAttemptsChanged;

        [SerializeField] private TaskType _currentTaskType;
        [SerializeField] private int _currentLevelIndex;
        [SerializeField] private int _currentQuestionIndex;
        [SerializeField] private int _correctAnswersCount;
        [SerializeField] private int _remainingAttempts;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartTask(TaskType taskType, int levelIndex, int taskIndex = 0)
        {
            Debug.Log($"[GameplayManager] Starting task: Type={taskType}, Level={levelIndex}, Task={taskIndex}");

            _currentTaskType = taskType;
            _currentLevelIndex = levelIndex;
            _currentQuestionIndex = taskIndex;
            _correctAnswersCount = 0;
            _remainingAttempts = MAX_ATTEMPTS;

            var levels = LevelSystem.Instance.GetLevelsFor(taskType);
            if (levels == null || levelIndex >= levels.Count)
            {
                Debug.LogError($"[GameplayManager] Level {levelIndex} not found for task type {taskType}");
                return;
            }

            var currentLevel = levels[levelIndex];
            if (currentLevel.miniCaseTasks == null || currentLevel.miniCaseTasks.Count == 0)
            {
                Debug.LogError($"[GameplayManager] No questions found in level {levelIndex}");
                return;
            }

            Debug.Log($"[GameplayManager] Found {currentLevel.miniCaseTasks.Count} questions in level {levelIndex}");
            OnAttemptsChanged?.Invoke(_remainingAttempts);
            LoadCurrentTask();
        }

        private void LoadCurrentTask()
        {
            var currentTask = GetCurrentTask();
            if (currentTask == null)
            {
                Debug.Log($"[GameplayManager] No more tasks available, completing level");
                CompleteLevel();
                return;
            }

            Debug.Log($"[GameplayManager] Loading task: {currentTask.GetQuestionText()}");
            OnTaskLoaded?.Invoke(currentTask);
        }

        public void SubmitAnswer(object answer)
        {
            var currentTask = GetCurrentTask();
            if (currentTask == null)
            {
                Debug.LogWarning("[GameplayManager] Cannot submit answer: no current task");
                return;
            }

            bool isCorrect = currentTask.ValidateAnswer(answer);
            Debug.Log($"[GameplayManager] Answer submitted: {(isCorrect ? "Correct" : "Incorrect")}");
            OnAnswerSubmitted?.Invoke(isCorrect);

            if (isCorrect)
            {
                _correctAnswersCount++;
            }

            _currentQuestionIndex++;
            LoadCurrentTask();
        }

        public void SkipTask()
        {
            if (!HasMoreTasks())
            {
                Debug.Log("[GameplayManager] Cannot skip: no more tasks available");
                return;
            }

            _remainingAttempts--;
            OnAttemptsChanged?.Invoke(_remainingAttempts);
            Debug.Log($"[GameplayManager] Task skipped. Remaining attempts: {_remainingAttempts}");
            OnTaskFailed?.Invoke();

            _currentQuestionIndex++;
            LoadCurrentTask();
        }

        private void CompleteLevel()
        {
            Debug.Log($"[GameplayManager] Completing level {_currentLevelIndex} with {_correctAnswersCount} correct answers");
            ProgressManager.Instance.CompleteLevel(_currentTaskType, _currentLevelIndex, _correctAnswersCount);
            OnTaskCompleted?.Invoke();
        }

        public ITaskData GetCurrentTask()
        {
            var levels = LevelSystem.Instance.GetLevelsFor(_currentTaskType);
            if (levels == null || _currentLevelIndex >= levels.Count)
            {
                Debug.LogWarning($"[GameplayManager] Invalid level index: {_currentLevelIndex}");
                return null;
            }

            var currentLevel = levels[_currentLevelIndex];
            if (_currentQuestionIndex >= currentLevel.miniCaseTasks.Count)
            {
                Debug.LogWarning($"[GameplayManager] Invalid question index: {_currentQuestionIndex}");
                return null;
            }

            return currentLevel.miniCaseTasks[_currentQuestionIndex];
        }

        public bool HasMoreTasks()
        {
            var levels = LevelSystem.Instance.GetLevelsFor(_currentTaskType);
            if (levels == null || _currentLevelIndex >= levels.Count)
            {
                Debug.LogWarning($"[GameplayManager] Invalid level index: {_currentLevelIndex}");
                return false;
            }

            var currentLevel = levels[_currentLevelIndex];
            bool hasMore = _currentQuestionIndex < currentLevel.miniCaseTasks.Count - 1;
            Debug.Log($"[GameplayManager] Has more tasks: {hasMore}");
            return hasMore;
        }

        public TaskType GetCurrentTaskType() => _currentTaskType;
        public int GetCurrentLevelIndex() => _currentLevelIndex;
        public int GetCurrentQuestionIndex() => _currentQuestionIndex;
        public int GetCorrectAnswersCount() => _correctAnswersCount;
        public int GetRemainingAttempts() => _remainingAttempts;
    }
}