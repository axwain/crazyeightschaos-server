using CrazyEights.PlayLib.Entities;
using CrazyEights.PlayLib.Enums;

namespace CrazyEights.Tests.PlayLib.Entities
{
    [TestFixture]
    public class DiscardPileTests
    {
        [Test, Description("Initializes Discard Pile")]
        public void DiscardPile_Initialization()
        {
            var cardCount = 1;
            var firstCard = new Card(Suits.Spade, 1, Effects.None, 0);
            var discardPile = new DiscardPile(firstCard, cardCount);
            Assert.That(discardPile.IsEmpty, Is.False, "Pile is not empty after initialization");
        }

        [Test, Description("Discard Pile Initialization with invalid card count")]
        public void DiscardPile_Initialization_InvalidCardCount()
        {
            var cardCount = -1;
            var firstCard = new Card(Suits.Spade, 1, Effects.None, 0);
            Assert.That(
                () => new DiscardPile(firstCard, cardCount),
                Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("totalCards"),
                "Discard Pile must be initialized with a card count greater than zero"
            );
        }

        [TestCase(Suits.Club, 1, Effects.DrawCards, 1)]
        [TestCase(Suits.Diamond, 2, Effects.InterchangeHands, -1)]
        [TestCase(Suits.Heart, 3, Effects.None, 0)]
        [TestCase(Suits.Spade, 4, Effects.ReverseTurnOrder, -1)]
        [TestCase(Suits.Star, 5, Effects.SkipTurn, 1)]
        [TestCase(Suits.Wild, 0, Effects.DrawCards, 4)]
        [Test, Description("Add Card to the pile")]
        public void DiscardPile_Add_Card(Suits suit, int value, Effects effect, int args)
        {
            var maxCardCount = 2;
            var firstCard = new Card(Suits.Spade, 1, Effects.None, 0);
            var card = new Card(suit, value, effect, args);
            var discardPile = new DiscardPile(firstCard, maxCardCount);

            discardPile.Add(card);
            var cards = discardPile.Clear();
            Assert.That(
                cards.Length,
                Is.EqualTo(2),
                $"should have added {suit} {value} to the discard pile"
            );
        }

        [Test, Description("Clears the discard pile")]
        public void DiscardPile_Clear()
        {
            var cardCount = 3;
            var firstCard = new Card(Suits.Spade, 1, Effects.None, 0);
            var wildCard = new Card(Suits.Wild, 0, Effects.None, 0);
            var lastCard = new Card(Suits.Club, 3, Effects.None, 0);
            var discardPile = new DiscardPile(firstCard, cardCount);

            discardPile.Add(wildCard);
            discardPile.Add(lastCard);

            var pile = discardPile.Clear();
            Card[] cards = { firstCard, wildCard, lastCard };

            Assert.That(discardPile.IsEmpty, Is.True, "Pile is empty after clearing");
            Assert.That(pile.Length, Is.EqualTo(cardCount), "Pile had three cards");
            for (var i = 0; i < cardCount; i++)
            {
                Assert.That(cards[i].Equals(pile[i]), Is.True, $"should had {pile[i].SuitId} {pile[i].Value} in the pile");
            }
        }
    }
}