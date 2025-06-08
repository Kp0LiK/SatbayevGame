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
        [SerializeField] private Profession _currentProfession;
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

        public void StartTask(TaskType taskType, Profession profession, int levelIndex, int taskIndex = 0)
        {
            Debug.Log($"[GameplayManager] Starting task: Type={taskType}, Profession={profession}, Level={levelIndex}, Task={taskIndex}");

            _currentTaskType = taskType;
            _currentProfession = profession;
            _currentLevelIndex = levelIndex;
            _currentQuestionIndex = taskIndex;
            _correctAnswersCount = 0;
            _remainingAttempts = MAX_ATTEMPTS;

            var currentLevel = LevelSystem.Instance.GetLevel(taskType, profession, levelIndex);
            if (currentLevel == null)
            {
                Debug.LogError($"[GameplayManager] Level {levelIndex} not found for task type {taskType} and profession {profession}");
                return;
            }

            var tasks = TaskFactory.GetTasks(currentLevel, taskType);
            if (tasks == null || tasks.Count == 0)
            {
                Debug.LogError($"[GameplayManager] No tasks found in level {levelIndex} for type {taskType} and profession {profession}");
                return;
            }

            Debug.Log($"[GameplayManager] Found {tasks.Count} tasks in level {levelIndex}");
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

            int correctCount = 0;
            bool isCorrect = false;

            if (answer is PairMatchWindowView.PairMatchResult pmr)
            {
                isCorrect = pmr.IsWin;
                correctCount = pmr.CorrectCount;
            }
            else
            {
                isCorrect = currentTask.ValidateAnswer(answer);
                correctCount = isCorrect ? 1 : 0;
            }

            Debug.Log($"[GameplayManager] Answer submitted: {(isCorrect ? "Correct" : "Incorrect")}, CorrectCount: {correctCount}");
            _correctAnswersCount += correctCount;

            OnAnswerSubmitted?.Invoke(isCorrect);

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
            LevelSystem.Instance.SaveLevelProgress(_currentTaskType, _currentProfession, _currentLevelIndex, _correctAnswersCount);
            if (_correctAnswersCount <= 0)
            {
                LoseLevel();
            }
            else
            {
                OnTaskCompleted?.Invoke();
            }
        }

        public void SuccessCompleteLevel(int correctAnswers)
        {
            _correctAnswersCount = correctAnswers;
            OnTaskCompleted?.Invoke();
        }

        public void LoseLevel()
        {
            OnTaskFailed?.Invoke();
        }

        public ITaskData GetCurrentTask()
        {
            var currentLevel = LevelSystem.Instance.GetLevel(_currentTaskType, _currentProfession, _currentLevelIndex);
            if (currentLevel == null)
            {
                Debug.LogWarning($"[GameplayManager] Invalid level index: {_currentLevelIndex}");
                return null;
            }

            var tasks = TaskFactory.GetTasks(currentLevel, _currentTaskType);
            if (tasks == null || _currentQuestionIndex >= tasks.Count)
            {
                Debug.LogWarning($"[GameplayManager] Invalid question index: {_currentQuestionIndex}");
                return null;
            }

            return tasks[_currentQuestionIndex];
        }

        public bool HasMoreTasks()
        {
            var currentLevel = LevelSystem.Instance.GetLevel(_currentTaskType, _currentProfession, _currentLevelIndex);
            if (currentLevel == null)
            {
                Debug.LogWarning($"[GameplayManager] Invalid level index: {_currentLevelIndex}");
                return false;
            }

            var tasks = TaskFactory.GetTasks(currentLevel, _currentTaskType);
            bool hasMore = tasks != null && _currentQuestionIndex < tasks.Count - 1;
            Debug.Log($"[GameplayManager] Has more tasks: {hasMore}");
            return hasMore;
        }

        public TaskType GetCurrentTaskType() => _currentTaskType;
        public TaskType SetCurrentTaskType(TaskType taskType) => _currentTaskType = taskType;
        public int GetCurrentLevelIndex() => _currentLevelIndex;
        public int SetCurrentLevelIndex(int levelIndex) => _currentLevelIndex = levelIndex;
        public Profession GetCurrentProfession() => _currentProfession;
        public Profession SetCurrentProfession(Profession profession) => _currentProfession = profession;
        public int GetCurrentQuestionIndex() => _currentQuestionIndex;
        public int GetCorrectAnswersCount() => _correctAnswersCount;
        public int GetRemainingAttempts() => _remainingAttempts;
    }
}