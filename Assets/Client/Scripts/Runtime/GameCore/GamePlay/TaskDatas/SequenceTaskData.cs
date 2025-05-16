using System;

namespace Client
{
    [Serializable]
    public class SequenceTaskData : ITaskData
    {
        public string[] steps;
        public string GetQuestionText() => "Расставь шаги по порядку:";
    }
}