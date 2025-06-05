using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "TaskData", menuName = "SatbayevQuest/TaskSet", order = 0)]
    public class TaskSet : ScriptableObject
    {
        public Profession Profession;
        public TaskType TaskType;

        public List<Level> Levels;
        
        [Button]
        public void SetLevelIndex()
        {
            var index = 0;
            foreach (var level in Levels)
            {
                level.LevelNumber = index;
                index++;
            }
        }
    }

    [System.Serializable]
    public class Level
    {
        public int LevelNumber;
        public bool IsLock;
        public List<MiniCaseTaskData> miniCaseTasks;
        public List<OddOneOutTaskData> oddOneOutTasks;
        public List<PairMatchTaskData> pairMatchTasks;
        public List<SequenceTaskData> sequenceTasks;

        public bool HasTasks()
        {
            return (miniCaseTasks != null && miniCaseTasks.Count > 0) ||
                   (oddOneOutTasks != null && oddOneOutTasks.Count > 0) ||
                   (pairMatchTasks != null && pairMatchTasks.Count > 0) ||
                   (sequenceTasks != null && sequenceTasks.Count > 0);
        }
    }
}