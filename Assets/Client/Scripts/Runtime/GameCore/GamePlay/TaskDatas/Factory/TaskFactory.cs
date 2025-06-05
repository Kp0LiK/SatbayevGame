using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public static class TaskFactory
    {
        private static readonly Dictionary<TaskType, System.Func<Level, List<ITaskData>>> _taskGetters = new()
        {
            { TaskType.MiniCase, level => level.miniCaseTasks?.ConvertAll(x => (ITaskData)x) },
            { TaskType.OddOneOut, level => level.oddOneOutTasks?.ConvertAll(x => (ITaskData)x) },
            { TaskType.PairMatch, level => level.pairMatchTasks?.ConvertAll(x => (ITaskData)x) },
            { TaskType.Sequence, level => level.sequenceTasks?.ConvertAll(x => (ITaskData)x) }
        };

        public static List<ITaskData> GetTasks(Level level, TaskType taskType)
        {
            return _taskGetters.TryGetValue(taskType, out var getter) ? getter(level) : null;
        }
    }
} 