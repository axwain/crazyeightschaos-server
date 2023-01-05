using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyEights.PlayLib.Entities
{
    public class DiscardPile
    {
        public bool IsEmpty { get { return Cards.Count == 0; } }
        private IList<Card> Cards { get; set; }

        public DiscardPile(Card firstCard, int totalCards)
        {
            if (totalCards > 0)
            {
                Cards = new List<Card>(totalCards);
                Cards.Add(firstCard);
            }
            else
            {
                throw new ArgumentException("totalCards must be greater than zero", "totalCards");
            }
        }

        public void Add(Card card)
        {
            Cards.Add(card);
        }

        public Card[] Clear()
        {
            var cards = Cards.ToArray();
            Cards.Clear();
            return cards;
        }
    }
}
