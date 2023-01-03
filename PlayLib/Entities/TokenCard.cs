using System;

using CrazyEights.PlayLib.Enums;

namespace CrazyEights.PlayLib.Entities
{
    // TODO: Create own tests
    public class TokenCard
    {
        public Suits SuitId { get; private set; }
        public int Value { get; private set; }

        internal TokenCard()
        {
            SuitId = Suits.Wild;
            Value = -1;
        }

        public void Update(Suits suit, int value)
        {
            if (suit == Suits.Wild)
            {
                throw new ArgumentOutOfRangeException("suit", "TokenCard can't be a Wild Card");
            }

            SuitId = suit;
            Value = value;
        }

        public bool Matches(Card card)
        {
            if (SuitId == Suits.Wild)
            {
                throw new InvalidOperationException("TokenCard must updated before a match");
            }

            bool isWild = card.SuitId == Suits.Wild;
            bool isSameSuit = card.SuitId == SuitId;
            bool isSameValue = card.Value == Value;

            return isWild || isSameSuit || isSameValue;
        }
    }
}