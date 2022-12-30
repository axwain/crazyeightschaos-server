namespace CrazyEights.Entities
{
    public class Deck
    {
        private readonly int PILE_SIZE = 10;
        private readonly int TOTAL_DEFAULT_SUITS = 4;
        private readonly int TOTAL_EXTENDED_SUITS = 5;
        private readonly int TOTAL_SUITS = 6;
        private readonly int WILD_COUNT = 4;
        private readonly int DEFAULT_COPIES_COUNT = 1;
        private readonly int EXTENDED_COPIES_COUNT = 2;
        private readonly int PLAYER_LIMIT_EXTENDED_SUITS = 5;
        private readonly int PLAYER_LIMIT_EXTENDED_COPIES = 3;

        private IDictionary<Suits, IList<Card>> LoadedCards { get; set; }
        private List<Card> Cards { get; set; }
        private Queue<Card> DiscardedPile { get; set; }

        private int CurrentCard { get; set; }
        private IList<Card> CardsToReshuffle { get; set; }

        public Deck(IList<int[]> wilds, IList<int[]> cards)
        {
            LoadedCards = new Dictionary<Suits, IList<Card>>(TOTAL_SUITS);
            Cards = new List<Card>(wilds.Count * 4 + cards.Count * TOTAL_EXTENDED_SUITS);
            DiscardedPile = new Queue<Card>(PILE_SIZE);
            CardsToReshuffle = new List<Card>(Cards.Count);

            LoadedCards.Add(Suits.Wild, CreateCardList(wilds, Suits.Wild));

            for (int i = 1; i < TOTAL_SUITS; i++)
            {
                var suit = (Suits)i;
                LoadedCards.Add(suit, CreateCardList(cards, suit));
            }
        }

        public void PrepareDeck(int numberOfPlayers)
        {
            Cards.Clear();

            var wilds = LoadedCards[Suits.Wild];
            foreach (var wild in wilds)
            {
                for (int i = 0; i < WILD_COUNT; i++)
                {
                    Cards.Add(wild);
                }
            }

            var totalSuits = numberOfPlayers < PLAYER_LIMIT_EXTENDED_SUITS ? TOTAL_DEFAULT_SUITS : TOTAL_EXTENDED_SUITS;
            var maxCopies = numberOfPlayers < PLAYER_LIMIT_EXTENDED_COPIES ? DEFAULT_COPIES_COUNT : EXTENDED_COPIES_COUNT;

            for (int i = 0; i < totalSuits; i++)
            {
                var suit = (Suits)(i + 1);
                var suitCards = LoadedCards[suit];
                foreach (var card in suitCards)
                {
                    if (card.Value == 0)
                    {
                        Cards.Add(card);
                    }
                    else
                    {
                        for (int j = 0; j < maxCopies; j++)
                        {
                            Cards.Add(card);
                        }
                    }
                }
            }
        }

        public void ShuffleDeck()
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

        public Card DrawCard()
        {
            CurrentCard--;
            var card = Cards[CurrentCard];

            if (CurrentCard < 1)
            {
                ShuffleDiscardedCards();
            }

            return card;
        }

        public void Discard(Card card)
        {
            if (DiscardedPile.Count == PILE_SIZE)
            {
                CardsToReshuffle.Add(DiscardedPile.Dequeue());
            }

            DiscardedPile.Enqueue(card);
        }

        // this should be executed only when current card is equal to 0
        private void ShuffleDiscardedCards()
        {
            Cards.Clear();
            Cards.AddRange(CardsToReshuffle);
            CardsToReshuffle.Clear();
            ShuffleDeck();
        }

        private IList<Card> CreateCardList(IList<int[]> rawCards, Suits suit)
        {
            var cardList = new List<Card>(rawCards.Count);
            foreach (var card in rawCards)
            {
                cardList.Add(new Card
                {
                    SuiteId = suit,
                    Value = card[0],
                    EffectId = (Effects)card[1],
                    EffectArg = card[2],
                });
            }
            return cardList;
        }
    }
}