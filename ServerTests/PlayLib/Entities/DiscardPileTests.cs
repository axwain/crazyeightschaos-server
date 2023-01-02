using CrazyEights.PlayLib.Entities;

namespace CrazyEights.Tests.Entities
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
            Assert.That(discardPile.TopCard.Equals(firstCard), Is.True, "Pile Top Card is equal to the first card");
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

        [Test, Description("Check if card can be added to pile")]
        public void DiscardPile_CanAdd_Card()
        {
            var cardCount = 1;
            var firstCard = new Card(Suits.Spade, 1, Effects.None, 0);
            var wildCard = new Card(Suits.Wild, 0, Effects.None, 0);
            var sameSuitCard = new Card(Suits.Spade, 5, Effects.None, 0);
            var sameValueCard = new Card(Suits.Star, 1, Effects.None, 0);
            var differentCardOne = new Card(Suits.Club, 5, Effects.None, 0);
            var differentCardTwo = new Card(Suits.Diamond, 2, Effects.None, 0);
            var differentCardThree = new Card(Suits.Heart, 3, Effects.SkipTurn, 0);
            var discardPile = new DiscardPile(firstCard, cardCount);

            Assert.That(discardPile.CanAdd(wildCard), Is.True, "Can add wild card");
            Assert.That(discardPile.CanAdd(sameSuitCard), Is.True, "Can add card with same suit");
            Assert.That(discardPile.CanAdd(sameValueCard), Is.True, "Can add card with same value");
            Assert.That(
                discardPile.CanAdd(differentCardOne),
                Is.False,
                "Can't that's not a wild, with same suit, or with same value"
            );
            Assert.That(
                discardPile.CanAdd(differentCardTwo),
                Is.False,
                "Can't that's not a wild, with same suit, or with same value"
            );
            Assert.That(
                discardPile.CanAdd(differentCardThree),
                Is.False,
                "Can't that's not a wild, with same suit, or with same value"
            );
        }

        [Test, Description("Add Card to the pile")]
        public void DiscardPile_Add_Card()
        {
            var cardCount = 6;
            var firstCard = new Card(Suits.Spade, 1, Effects.None, 0);
            var wildCard = new Card(Suits.Wild, 0, Effects.None, 0);
            var cardAfterWild = new Card(Suits.Club, 3, Effects.None, 0);
            var sameSuitCard = new Card(Suits.Club, 5, Effects.None, 0);
            var sameValueCard = new Card(Suits.Star, 5, Effects.None, 0);
            var discardPile = new DiscardPile(firstCard, cardCount);

            discardPile.Add(wildCard);
            Assert.That(
                discardPile.TopCard.Equals(wildCard),
                Is.True,
                "Wild card was added at the top of the pile"
            );

            discardPile.Add(wildCard);
            Assert.That(
                discardPile.TopCard.Equals(wildCard),
                Is.True,
                "A wild card after a wild card was added at the top of the pile"
            );

            discardPile.Add(cardAfterWild);
            Assert.That(
                discardPile.TopCard.Equals(cardAfterWild),
                Is.True,
                "A card after a wild was added at the top of the pile"
            );

            discardPile.Add(sameSuitCard);
            Assert.That(
                discardPile.TopCard.Equals(sameSuitCard),
                Is.True,
                "A card with the same suit was added at the top of the pile"
            );

            discardPile.Add(sameValueCard);
            Assert.That(
                discardPile.TopCard.Equals(sameValueCard),
                Is.True,
                "A card with the same value was added at the top of the pile"
            );
        }

        [Test, Description("Can't Add An Invalid Card to the pile")]
        public void DiscardPile_Add_Invalid_Card()
        {
            var cardCount = 1;
            var firstCard = new Card(Suits.Spade, 1, Effects.None, 0);
            var differentCardOne = new Card(Suits.Club, 2, Effects.None, 0);
            var differentCardTwo = new Card(Suits.Diamond, 3, Effects.None, 0);
            var differentCardThree = new Card(Suits.Heart, 4, Effects.None, 0);
            var discardPile = new DiscardPile(firstCard, cardCount);

            Assert.That(
                () => discardPile.Add(differentCardOne),
                Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("card"),
                "Discard Pile must be initialized with a card count greater than zero"
            );

            Assert.That(
                () => discardPile.Add(differentCardTwo),
                Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("card"),
                "Discard Pile must be initialized with a card count greater than zero"
            );

            Assert.That(
                () => discardPile.Add(differentCardThree),
                Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("card"),
                "Discard Pile must be initialized with a card count greater than zero"
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

            var totalDiscarded = discardPile.Clear();

            Assert.That(discardPile.IsEmpty, Is.True, "Pile is empty after clearing");
            Assert.That(totalDiscarded, Is.EqualTo(cardCount), "Pile had three cards");
        }
    }
}