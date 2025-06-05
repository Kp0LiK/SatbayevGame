using UnityEngine;

namespace Client
{
    public abstract class BaseTaskData : ITaskData
    {
        [TextArea(2, 3)] public string questionText;
        
        public string GetQuestionText() => questionText;
        public abstract TaskType GetTaskType();
        public abstract bool ValidateAnswer(object answer);
    }
} 