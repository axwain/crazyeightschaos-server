using CrazyEights.PlayLib.Entities;
using CrazyEights.PlayLib.Enums;

namespace CrazyEights.Tests.PlayLib.Entities
{
    [TestFixture]
    public class TopCardTests
    {
        [Test, Description("Initializes Top Card")]
        public void TopCard_Constructor()
        {
            var TopCard = new TopCard();
            Assert.That(TopCard.SuitId, Is.EqualTo(Suits.Wild), "Top Card starts with wild suit");
            Assert.That(TopCard.Value, Is.EqualTo(-1), "Top Card starts with a -1 value");
        }

        [TestCase(Suits.Club, 1)]
        [TestCase(Suits.Diamond, 3)]
        [TestCase(Suits.Heart, 5)]
        [TestCase(Suits.Spade, 8)]
        [TestCase(Suits.Star, 10)]
        [Description("Updates TopCard suit and value")]
        public void TopCard_Update(Suits suit, int value)
        {
            var TopCard = new TopCard();
            TopCard.Update(suit, value);
            Assert.That(TopCard.SuitId, Is.EqualTo(suit), $"should have updated its SuitId to {suit}");
            Assert.That(TopCard.Value, Is.EqualTo(value), $"should have updated its Value to {value}");
        }

        [Test, Description("TopCard can't be wild after update")]
        public void TopCard_Update_InvalidSuit()
        {
            var TopCard = new TopCard();
            Assert.That(
                () => TopCard.Update(Suits.Wild, 0),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("suit"),
                "should throw if updating to wild suit");
        }

        [TestCase(Suits.Club, 1, 2)]
        [TestCase(Suits.Diamond, 3, 5)]
        [TestCase(Suits.Spade, 8, -1)]
        [Description("TopCard matches suit of another card")]
        public void TopCard_Matches_Suit(Suits suit, int topValue, int cardValue)
        {
            var TopCard = new TopCard();
            TopCard.Update(suit, topValue);
            var card = new Card(suit, topValue, 0, 0);
            Assert.That(
                TopCard.Matches(card),
                Is.True,
                $"should have matched suit {TopCard.SuitId} to {card.SuitId}"
            );
        }

        [TestCase(Suits.Club, Suits.Heart, 1)]
        [TestCase(Suits.Diamond, Suits.Star, 5)]
        [TestCase(Suits.Spade, Suits.Club, 8)]
        [Description("TopCard matches value of another card")]
        public void TopCard_Matches_Value(Suits topSuit, Suits cardSuit, int value)
        {
            var TopCard = new TopCard();
            TopCard.Update(topSuit, value);
            var card = new Card(cardSuit, value, 0, 0);
            Assert.That(
                TopCard.Matches(card),
                Is.True,
                $"should have matched value {TopCard.Value} to {card.Value}"
            );
        }

        [TestCase(Suits.Club, 1, 2)]
        [TestCase(Suits.Diamond, 3, 4)]
        [TestCase(Suits.Heart, 5, 6)]
        [TestCase(Suits.Spade, 7, 8)]
        [TestCase(Suits.Star, 10, 10)]
        [Description("TopCard matches with a wild card")]
        public void TopCard_Matches_WildSuit(Suits suit, int topValue, int cardValue)
        {
            var TopCard = new TopCard();
            TopCard.Update(suit, topValue);
            var card = new Card(Suits.Wild, topValue, 0, 0);
            Assert.That(
                TopCard.Matches(card),
                Is.True,
                $"should have matched {TopCard.SuitId} to Wild"
            );
        }

        [Test, Description("TopCard match throws if not updated after construction")]
        public void TopCard_Matches_NoUpdate()
        {
            var TopCard = new TopCard();
            var card = new Card(Suits.Wild, 8, 0, 0);
            Assert.That(
                () => TopCard.Matches(card),
                Throws
                    .TypeOf<InvalidOperationException>()
                    .With
                    .Message
                    .EqualTo("TopCard must be updated after construction and before a match"),
                "should throw if it hasn't been updated after construction"
            );
        }
    }
}