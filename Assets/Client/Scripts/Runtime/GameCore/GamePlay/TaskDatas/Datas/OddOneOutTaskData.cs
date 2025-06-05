using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class TaskOption
    {
        public string text;
        public Sprite sprite;
    }

    [Serializable]
    public class OddOneOutTaskData : BaseTaskData, ITaskData
    {
        [TextArea(2, 3)] public string questionText;
        public List<TaskOption> options;
        public int oddIndex;

        public override TaskType GetTaskType() => TaskType.OddOneOut;
        public new string GetQuestionText() => questionText;
        
        public override bool ValidateAnswer(object answer)
        {
            if (answer is int selectedIndex)
            {
                return selectedIndex == oddIndex;
            }
            return false;
        }
    }
}