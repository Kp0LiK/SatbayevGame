using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class OddOneOutTaskData : ITaskData
    {
        [TextArea(2, 3)] public string questionText;
        public string[] options;
        public int oddIndex;

        public string GetQuestionText() => questionText;
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