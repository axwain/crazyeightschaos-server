using System;

using CrazyEights.PlayLib.Data;
using CrazyEights.PlayLib.Enums;

namespace CrazyEights.PlayLib.Entities
{
    public class Card
    {
        public Suits SuitId { get; private set; }
        public int Value { get; private set; }
        public Effects EffectId { get; private set; }
        public int EffectArg { get; private set; }
        public int MaxBaseCopies { get; private set; }
        public int MaxExtendedCopies { get; private set; }

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
            MaxBaseCopies = 1;
            MaxExtendedCopies = 2;
        }

        public Card(Suits suit, int value, CardData definition)
        {
            SuitId = suit;
            Value = value;
            EffectId = definition.EffectId;
            EffectArg = definition.EffectArgs;
            MaxBaseCopies = definition.MaxBaseCopies;
            MaxExtendedCopies = definition.MaxExtendedCopies;
        }

        public bool Equals(Card card)
        {
            return SuitId == card.SuitId && Value == card.Value;
        }
    }
}