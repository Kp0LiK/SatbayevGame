using System;

namespace Client
{
    [Serializable]
    public class PairMatchTaskData : ITaskData
    {
        public string[] leftColumn;
        public string[] rightColumn;
        public string GetQuestionText() => "Соедини пары:";
    }
}