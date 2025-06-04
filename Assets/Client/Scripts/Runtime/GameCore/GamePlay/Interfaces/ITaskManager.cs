using System;

namespace Client
{
    public interface ITaskManager
    {
        event Action<ITaskData> OnTaskLoaded;
        event Action<bool> OnAnswerSubmitted;
        event Action OnTaskCompleted;
        event Action OnTaskFailed;

        void StartTask(TaskType taskType, int levelIndex, int taskIndex);
        void SubmitAnswer(object answer);
        void SkipTask();
        ITaskData GetCurrentTask();
        bool HasMoreTasks();
    }
} 