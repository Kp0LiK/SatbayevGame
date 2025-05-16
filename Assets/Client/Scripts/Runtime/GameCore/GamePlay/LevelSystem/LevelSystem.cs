using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class LevelSystem : MonoBehaviour
    {
        [SerializeField] private List<LevelSetData> _allLevelSets;

        public List<Level> GetLevelsFor(TaskType taskType)
        {
            return _allLevelSets.Find(set => set.taskType == taskType)?.levels;
        }
    }
}