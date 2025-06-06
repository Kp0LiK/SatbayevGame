using System;

namespace Client
{
    public interface ITaskManager
    {
        event Action<ITaskData> OnTaskLoaded;
        event Action<bool> OnAnswerSubmitted;
        event Action OnTaskCompleted;
        event Action OnTaskFailed;

        void StartTask(TaskType taskType, Profession profession, int levelIndex, int taskIndex = 0);
        void SubmitAnswer(object answer);
        void SkipTask();
        ITaskData GetCurrentTask();
        bool HasMoreTasks();
        TaskType GetCurrentTaskType();
        Profession GetCurrentProfession();
        int GetCurrentLevelIndex();
    }
} 