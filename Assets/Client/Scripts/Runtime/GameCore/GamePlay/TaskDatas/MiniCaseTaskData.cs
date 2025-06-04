using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class MiniCaseTaskData : ITaskData
    {
        [TextArea(2, 3)] public string questionText;
        public string[] answerOptions;
        public int correctAnswerIndex;

        public string GetQuestionText() => questionText;
        
        public TaskType GetTaskType() => TaskType.MiniCase;
        
        public bool ValidateAnswer(object answer)
        {
            if (answer is int selectedIndex)
            {
                return selectedIndex == correctAnswerIndex;
            }
            return false;
        }
    }
}