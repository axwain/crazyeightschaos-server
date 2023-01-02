using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyEights.PlayLib.Entities
{
    public class DiscardPile
    {
        public Card TopCard { get; private set; }
        public bool IsEmpty { get { return Cards.Count == 0; } }
        private IList<Card> Cards { get; set; }

        public DiscardPile(Card firstCard, int totalCards)
        {
            if (totalCards > 0)
            {
                Cards = new List<Card>(totalCards);
                AddCard(firstCard);
            }
            else
            {
                throw new ArgumentException("totalCards must be greater than zero", "totalCards");
            }
        }

        public bool CanAdd(Card card)
        {
            bool isWild = card.SuiteId == Suits.Wild || TopCard.SuiteId == Suits.Wild;
            bool isSameSuit = card.SuiteId == TopCard.SuiteId;
            bool isSameValue = card.Value == TopCard.Value;

            return isWild || isSameSuit || isSameValue;
        }

        public void Add(Card card)
        {
            if (CanAdd(card))
            {
                AddCard(card);
            }
            else
            {
                throw new ArgumentException(
                    $"Can't add card {card.SuiteId} {card.Value} over {TopCard.SuiteId} {TopCard.Value} to the discard pile",
                    "card"
                );
            }
        }

        public int Clear()
        {
            var size = Cards.Count;
            Cards.Clear();
            return size;
        }

        private void AddCard(Card card)
        {
            Cards.Add(card);
            TopCard = card;
        }
    }
}
