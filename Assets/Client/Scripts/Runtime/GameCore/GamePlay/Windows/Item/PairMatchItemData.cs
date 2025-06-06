using UnityEngine;

namespace Client
{
    public class PairMatchItemData
    {
        public string Id { get; private set; }
        public string Text { get; private set; }
        public bool IsMatched { get; set; }
        public bool IsLeft { get; private set; }

        public PairMatchItemData(string id, string text, bool isLeft)
        {
            Id = id;
            Text = text;
            IsLeft = isLeft;
            IsMatched = false;
        }
    }
} 