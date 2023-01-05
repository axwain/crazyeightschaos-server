using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyEights.PlayLib.Entities
{
    //TODO: Extract TopCard property
    public class DiscardPile
    {
        public TopCard TopCard { get; private set; }
        public bool IsEmpty { get { return Cards.Count == 0; } }
        private IList<Card> Cards { get; set; }

        public DiscardPile(Card firstCard, int totalCards)
        {
            if (totalCards > 0)
            {
                Cards = new List<Card>(totalCards);
                Cards.Add(firstCard);
                TopCard = new TopCard();
            }
            else
            {
                throw new ArgumentException("totalCards must be greater than zero", "totalCards");
            }
        }

        public void Add(Card card)
        {
            if (TopCard.Matches(card))
            {
                Cards.Add(card);
            }
            else
            {
                throw new ArgumentException(
                    $"Can't add card {card.SuitId} {card.Value} over {TopCard.SuitId} {TopCard.Value} to the discard pile",
                    "card"
                );
            }
        }

        public Card[] Clear()
        {
            var cards = Cards.ToArray();
            Cards.Clear();
            return cards;
        }
    }
}
