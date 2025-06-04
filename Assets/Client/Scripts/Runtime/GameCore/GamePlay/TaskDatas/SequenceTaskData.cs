using System;

namespace Client
{
    [Serializable]
    public class SequenceTaskData : ITaskData
    {
        public string[] steps;
        public string GetQuestionText() => "Расставь шаги по порядку:";
        public TaskType GetTaskType()
        {
            throw new NotImplementedException();
        }

        public bool ValidateAnswer(object answer)
        {
            throw new NotImplementedException();
        }
    }
}