using System;

using CrazyEights.PlayLib.Enums;

namespace CrazyEights.PlayLib.Entities
{
    public class Card
    {
        public Suits SuitId { get; private set; }
        public int Value { get; private set; }
        public Effects EffectId { get; private set; }
        public int EffectArg { get; private set; }

        public Card(Suits suit, int value, Effects effect, int effectArg)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value", "Value can't be negative");
            }

            SuitId = suit;
            Value = value;
            EffectId = effect;
            EffectArg = effectArg;
        }

        public bool Equals(Card card)
        {
            return SuitId == card.SuitId && Value == card.Value;
        }
    }
}