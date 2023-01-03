using System;
using System.Collections.Generic;

namespace CrazyEights.PlayLib.Data
{
    public class DeckDefinition
    {
        public IList<CardData> Suits { get; private set; }
        public IList<CardData> Wilds { get; private set; }

        public DeckDefinition(
            IList<CardData> suits,
            IList<CardData> wilds
        )
        {
            if (suits.Count < 1)
            {
                throw new ArgumentException("The number of suits is less than 1", "suitsCount");
            }
            Suits = suits;
            Wilds = wilds;
        }
    }
}