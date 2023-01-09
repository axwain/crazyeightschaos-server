using CrazyEights.PlayLib.Entities;
using CrazyEights.PlayLib.Enums;

namespace CrazyEights.Tests.PlayLib.Entities
{
    [TestFixture]
    public class HandTests
    {
        private readonly Card sampleCard = new Card(Suits.Spade, 1, Effects.None, 0);

        [Test, Description("Initializes Hand with a size of 0")]
        public void Hand_Constructor()
        {
            var hand = new Hand();
            Assert.That(hand.Size, Is.EqualTo(0), "should start with empty hand");
        }

        [Test, Description("Adds cards to the Hand with unique ids")]
        public void Hand_AddCard_UniqueId()
        {
            var hand = new Hand();
            var totalCards = 5;

            for (var i = 0; i < totalCards; i++)
            {
                Assert.That(
                    hand.AddCard(i, sampleCard),
                    Is.True,
                    $"should add {sampleCard.SuitId} {sampleCard.Value} with id {i}"
                );

                Assert.That(
                    hand.PeekCard(i),
                    Is.EqualTo(sampleCard),
                    $"should have {sampleCard.SuitId} {sampleCard.Value} with id {i}"
                );
            }

            for (var i = 0; i < totalCards; i++)
            {
                Assert.That(
                    hand.RemoveCard(i),
                    Is.True,
                    $"should remove {sampleCard.SuitId} {sampleCard.Value} with id {i}"
                );

                Assert.That(
                    () => hand.PeekCard(i),
                    Throws.TypeOf<KeyNotFoundException>(),
                    $"should not throw trying to get id {i}"
                );
            }

            Assert.That(hand.Size, Is.EqualTo(0), $"should have {totalCards} cards");
        }

        [Test, Description("Remove cards from the Hand with unique ids")]
        public void Hand_RemoveCard_UniqueId()
        {
            var hand = new Hand();
            var totalCards = 5;

            for (var i = 0; i < totalCards; i++)
            {
                hand.AddCard(i, sampleCard);
            }

            Assert.That(hand.Size, Is.EqualTo(totalCards), $"should have {totalCards} cards");
        }

        [Test, Description("Can't add cards to the Hand with duplicate ids")]
        public void Hand_AddCard_DuplicatedId()
        {
            var hand = new Hand();
            var duplicateCard = new Card(Suits.Heart, 5, Effects.None, 0);
            var id = 1;

            hand.AddCard(id, sampleCard);
            Assert.That(hand.Size, Is.EqualTo(1), "should have one card");
            Assert.That(
                    hand.PeekCard(id),
                    Is.EqualTo(sampleCard),
                    $"should have {sampleCard.SuitId} {sampleCard.Value} with id {id}"
                );

            Assert.That(
                hand.AddCard(id, duplicateCard),
                Is.False,
                $"should have not added a card with a duplicated id {id}"
            );
            Assert.That(hand.Size, Is.EqualTo(1), "should still have one card");
            Assert.That(
                    hand.PeekCard(id),
                    Is.EqualTo(sampleCard),
                    $"should have not changed card id {id} from {sampleCard.SuitId} {sampleCard.Value}"
                );
        }

        [Test, Description("Hand doesn't change after trying to remove a card with an invalid id")]
        public void Hand_RemoveCard_invalidId()
        {
            var hand = new Hand();
            var id = 1;

            hand.AddCard(id, sampleCard);
            Assert.That(hand.Size, Is.EqualTo(1), "should have one card");
            Assert.That(
                    hand.PeekCard(id),
                    Is.EqualTo(sampleCard),
                    $"should have {sampleCard.SuitId} {sampleCard.Value} with id {id}"
                );

            Assert.That(
                hand.RemoveCard(0),
                Is.False,
                $"should have not removed card with a invalid id {0}"
            );
            Assert.That(hand.Size, Is.EqualTo(1), "should still have one card");
            Assert.That(
                    hand.PeekCard(id),
                    Is.EqualTo(sampleCard),
                    $"should have not changed card id {id} from {sampleCard.SuitId} {sampleCard.Value}"
                );
        }
    }
}