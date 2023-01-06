using CrazyEights.PlayLib.Data;
using CrazyEights.PlayLib.Entities;
using CrazyEights.PlayLib.Enums;
using CrazyEights.PlayLib.Utils;

namespace CrazyEights.Tests.PlayLib.Entities
{
    [TestFixture]
    public class DeckTests
    {
        private DeckDefinition DoubleDeck
        {
            get
            {
                var path = "./Assets/DeckDefinitions/doubleDeck.json";
                var jsonString = File.ReadAllText(path);
                return DeckDefinitionLoader.Load<DeckDefinition>(jsonString);
            }
        }

        private DeckDefinition ValidDeck
        {
            get
            {
                var path = "./Assets/DeckDefinitions/validDeck.json";
                var jsonString = File.ReadAllText(path);
                return DeckDefinitionLoader.Load<DeckDefinition>(jsonString);
            }
        }

        //Interchanges odds with their next previous even
        private Func<int, int> IntercalateInts
        {
            get
            {
                Func<Func<int, int>> getInt = () =>
                {
                    var counter = 0;
                    return (int maxCount) =>
                    {
                        if (counter >= maxCount)
                        {
                            counter = 0;
                        }
                        var next = (++counter % 2) - 1 + counter;
                        return next == maxCount ? next - 1 : next;
                    };
                };

                return getInt();
            }
        }

        private Func<int, int> ReturnCurrent
        {
            get
            {
                Func<Func<int, int>> getInt = () =>
                {
                    var counter = 0;
                    return (int maxCount) =>
                    {
                        if (counter >= maxCount)
                        {
                            counter = 0;
                        }
                        return counter++;
                    };
                };

                return getInt();
            }
        }

        [Test, Description("Can't prepare Deck with invalid number of players")]
        public void Deck_Prepare_InvalidNumberOfPlayers()
        {
            var deck = new Deck(ValidDeck, ReturnCurrent);
            deck.Prepare(2);

            Assert.That(
                () => deck.Prepare(1),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("numberOfPlayers"),
                "should throw if number of players is less than 2"
            );

            Assert.That(
                () => deck.Prepare(8),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("numberOfPlayers"),
            "should throw if number of players is more than 7"
            );
        }

        [Test, Description("Initializes Deck for two players and draws from it")]
        public void Deck_Draw_After_PrepareShuffle()
        {
            var deck = new Deck(ValidDeck, ReturnCurrent);
            deck.Prepare(2);
            deck.Shuffle();

            for (var i = 5; i > 0; i--)
            {
                var suit = (Suits)i;
                var card = deck.Draw();
                Assert.That(
                    card.Equals(new Card(suit, 0, 0, 0)),
                    Is.True,
                    $"should have drawn {suit} 0 and got {card.SuitId} {card.Value}"
                );
            }
        }

        [Test, Description("Initializes Deck for four players and draws from it")]
        public void Deck_Draw_ForFourPlayers()
        {
            var maxCopies = 2;
            var deck = new Deck(DoubleDeck, ReturnCurrent);
            deck.Prepare(4);
            deck.Shuffle();

            for (var i = 5; i > 0; i--)
            {
                var suit = (Suits)i;
                for (var value = 1; value >= 0; value--)
                {
                    for (var j = 0; j < maxCopies; j++)
                    {
                        var card = deck.Draw();
                        Assert.That(
                            card.Equals(new Card(suit, value, 0, 0)),
                            Is.True,
                            $"should have drawn {suit} {value} and got {card.SuitId} {card.Value}"
                        );
                    }
                }
            }
        }

        [Test, Description("Initializes Deck for seven players and draws from it")]
        public void Deck_Draw_ForSevenPlayers()
        {
            var maxCopies = 2;
            var deck = new Deck(DoubleDeck, ReturnCurrent);
            deck.Prepare(7);
            deck.Shuffle();

            for (var i = 5; i >= 0; i--)
            {
                var suit = (Suits)i;
                for (var value = 1; value >= 0; value--)
                {
                    for (var j = 0; j < maxCopies; j++)
                    {
                        var card = deck.Draw();
                        Assert.That(
                            card.Equals(new Card(suit, value, 0, 0)),
                            Is.True,
                            $"should have drawn {suit} {value} and got {card.SuitId} {card.Value}"
                        );
                    }
                }
            }
        }

        [Test, Description("Can't draw before shuffling")]
        public void Deck_Draw_BeforeShuffle()
        {
            var deck = new Deck(ValidDeck, ReturnCurrent);
            deck.Prepare(2);

            Assert.That(
                () => deck.Draw(),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Can't draw card from empty deck"),
                "Can't draw before shuffling cards. Deck is empty"
            );
        }

        [Test, Description("Can't draw after drawing all cards")]
        public void Deck_Draw_AfterAllCards()
        {
            var deck = new Deck(ValidDeck, ReturnCurrent);
            deck.Prepare(2);
            deck.Shuffle();

            for (var i = 0; i < 5; i++)
            {
                deck.Draw();
            }

            Assert.That(
                () => deck.Draw(),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("Can't draw card from empty deck"),
                "Can't draw before shuffling cards. Deck is empty"
            );
        }

        [Test, Description("Initializes Deck for two players and draws from it using provided getRandomInt")]
        public void Deck_Shuffle_WithProvidedGetRandomInt()
        {
            var deck = new Deck(ValidDeck, IntercalateInts);
            deck.Prepare(2);
            deck.Shuffle();

            Suits[] suits = { Suits.Wild, Suits.Heart, Suits.Diamond, Suits.Spade, Suits.Club };
            foreach (var suit in suits)
            {
                var card = deck.Draw();
                Assert.That(
                    card.Equals(new Card(suit, 0, 0, 0)),
                    Is.True,
                    $"should have drawn {suit} 0 and got {card.SuitId} {card.Value}"
                );
            }
        }

        [Test, Description("Reshuffle cards")]
        public void Deck_Reshuffle()
        {
            var deck = new Deck(ValidDeck, ReturnCurrent);
            deck.Prepare(2);
            deck.Shuffle();
            var pile = new DiscardPile(deck.Draw(), 5);

            for (var i = 0; i < 4; i++)
            {
                pile.Add(deck.Draw());
            }

            deck.Reshuffle(pile.Clear());

            Suits[] suits = { Suits.Spade, Suits.Club, Suits.Heart, Suits.Diamond, Suits.Wild };
            foreach (var suit in suits)
            {
                var card = deck.Draw();
                Assert.That(
                    card.Equals(new Card(suit, 0, 0, 0)),
                    Is.True,
                    $"should have drawn {suit} 0 and got {card.SuitId} {card.Value}"
                );
            }
        }
    }
}