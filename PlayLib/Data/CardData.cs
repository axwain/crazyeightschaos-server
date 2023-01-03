using System;

using CrazyEights.PlayLib.Enums;

namespace CrazyEights.PlayLib.Data
{
    public class CardData
    {
        public Effects EffectId { get; private set; }
        public int EffectArgs { get; private set; }
        public int MaxBaseCopies { get; private set; }
        public int MaxExtendedCopies { get; private set; }

        public CardData(
            Effects effectId,
            int effectArgs,
            int maxBaseCopies,
            int maxExtendedCopies
        )
        {
            if (maxBaseCopies < 1)
            {
                throw new ArgumentOutOfRangeException(
                    "maxBaseCopies",
                    "Max Base Copies should not be less than one"
                );
            }

            if (maxExtendedCopies < 1)
            {
                throw new ArgumentOutOfRangeException(
                    "maxExtendedCopies",
                    "Max Extended Copies should not be less than one"
                );
            }

            if (!Enum.IsDefined(typeof(Effects), effectId))
            {
                throw new ArgumentOutOfRangeException(
                    "effectId",
                    "Effect Id has an invalid value"
                );
            }

            EffectId = effectId;
            EffectArgs = effectArgs;
            MaxBaseCopies = maxBaseCopies;
            MaxExtendedCopies = maxExtendedCopies;
        }
    }
}