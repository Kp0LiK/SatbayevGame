using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "LevelSet", menuName = "SatbayevQuest/LevelSet", order = 0)]
    public class LevelSetData : ScriptableObject
    {
        public TaskType taskType;
        public List<Level> levels;

        [Button]
        public void SetLevelIndex()
        {
            var index = 0;
            foreach (var level in levels)
            {
                level.LevelNumber = index;
                index++;
            }
        }
    }

    [Serializable]
    public class Level
    {
        public int LevelNumber;
        public bool IsLock;
    }
}