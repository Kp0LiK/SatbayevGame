using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "TaskData", menuName = "SatbayevQuest/TaskSet", order = 0)]
    public class TaskSet : ScriptableObject
    {
        public Profession profession;
        public TaskType taskType;

        public List<MiniCaseTaskData> miniCaseTasks;
        public List<OddOneOutTaskData> oddOneOutTasks;
        public List<PairMatchTaskData> pairMatchTasks;
        public List<SequenceTaskData> sequenceTasks;
    }
}