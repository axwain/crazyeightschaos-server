using System;
using System.Collections.Generic;

using CrazyEights.PlayLib.Data;
using CrazyEights.PlayLib.Enums;

namespace CrazyEights.PlayLib.Entities
{
    public class Deck
    {
        private readonly int TOTAL_EXTENDED_SUITS = 5;
        private readonly int TOTAL_SUITS = 5;
        private readonly int PLAYER_LIMIT_EXTENDED_SUITS = 5;
        private readonly int PLAYER_LIMIT_EXTENDED_COPIES = 3;

        private IDictionary<Suits, IList<Card>> LoadedCards { get; set; }
        private List<Card> Cards { get; set; }

        private int CurrentCard { get; set; }

        public Deck(DeckDefinition definition)
        {
            LoadedCards = new Dictionary<Suits, IList<Card>>(TOTAL_SUITS + 1);
            var maxCardCount = definition.WildsMaxCount + definition.SuitsMaxCount * TOTAL_EXTENDED_SUITS;
            Cards = new List<Card>(maxCardCount);

            LoadedCards.Add(Suits.Wild, CreateCardList(definition.Wilds, Suits.Wild));

            for (int i = 0; i < TOTAL_SUITS; i++)
            {
                var suit = (Suits)i;
                LoadedCards.Add(suit, CreateCardList(definition.Suits, suit));
            }
        }

        public void PrepareDeck(int numberOfPlayers)
        {
            Cards.Clear();

            var wilds = LoadedCards[Suits.Wild];
            foreach (var wild in wilds)
            {
                var wildCount = ShouldUseBaseCopies(numberOfPlayers) ? wild.MaxBaseCopies : wild.MaxExtendedCopies;
                for (int i = 0; i < wildCount; i++)
                {
                    Cards.Add(wild);
                }
            }

            var startSuit = ShouldUseBaseSuits(numberOfPlayers) ? Suits.Spade : Suits.Star;

            for (int i = (int)startSuit; i < TOTAL_SUITS; i++)
            {
                var suit = (Suits)(i + 1);
                var suitCards = LoadedCards[suit];
                foreach (var card in suitCards)
                {
                    var maxCopies = ShouldUseBaseCopies(numberOfPlayers) ? card.MaxBaseCopies : card.MaxExtendedCopies;
                    for (int j = 0; j < maxCopies; j++)
                    {
                        Cards.Add(card);
                    }
                }
            }
        }

        public void Shuffle()
        {
            var rndNumber = new Random();
            for (int i = 0; i < Cards.Count; i++)
            {
                var next = rndNumber.Next(Cards.Count);
                var card = Cards[i];
                Cards[i] = Cards[next];
                Cards[next] = card;
            }

            CurrentCard = Cards.Count;
        }

        public void Reshuffle(Card[] cards)
        {
            Cards.Clear();
            Cards.AddRange(cards);
            Shuffle();
        }

        public Card DrawCard()
        {
            if (CurrentCard <= 0)
            {
                throw new InvalidOperationException("Can't draw card from empty deck");
            }

            CurrentCard--;
            var card = Cards[CurrentCard];

            return card;
        }

        private IList<Card> CreateCardList(IList<CardData> rawCards, Suits suit)
        {
            var cardList = new List<Card>(rawCards.Count);
            for (var i = 0; i < rawCards.Count; i++)
            {
                cardList.Add(new Card(suit, i, rawCards[i]));
            }

            return cardList;
        }

        private bool ShouldUseBaseCopies(int numberOfPlayers)
        {
            return numberOfPlayers < PLAYER_LIMIT_EXTENDED_COPIES;
        }

        private bool ShouldUseBaseSuits(int numberOfPlayers)
        {
            return numberOfPlayers < PLAYER_LIMIT_EXTENDED_SUITS;
        }
    }
}