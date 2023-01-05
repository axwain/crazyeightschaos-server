using System.Text.Json;

using CrazyEights.PlayLib.Data;
using CrazyEights.PlayLib.Utils;

namespace CrazyEights.Tests.PlayLib.Utils
{
    [TestFixture]
    public class DeckDefinitionLoaderTests
    {
        public static bool CardDataIsEqual(CardData A, CardData B)
        {
            return A.EffectId == B.EffectId
                && A.EffectArgs == B.EffectArgs
                && A.MaxBaseCopies == B.MaxBaseCopies
                && A.MaxExtendedCopies == B.MaxExtendedCopies;
        }

        [Test, Description("Loads cards from a json string")]
        public void DeckDefinitionLoader_Load_ValidDeck()
        {
            var validDeckPath = "./Assets/DeckDefinitions/validDeck.json";
            Assert.That(File.Exists(validDeckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(validDeckPath);
            var deckDefinition = DeckDefinitionLoader.Load<DeckDefinition>(jsonString);

            Assert.That(
                deckDefinition.Suits.Count,
                Is.EqualTo(1),
                "It should have defined one card per suit"
            );

            Assert.That(
                CardDataIsEqual(deckDefinition.Suits[0], new CardData(0, 0, 1, 2)),
                Is.True,
                "It has loaded correctly the card data"
            );

            Assert.That(
                deckDefinition.Wilds.Count,
                Is.EqualTo(1),
                "It should have defined one wild card"
            );

            Assert.That(
                CardDataIsEqual(deckDefinition.Wilds[0], new CardData(0, 0, 1, 2)),
                Is.True,
                "It has loaded correctly the wild card data"
            );

            Assert.That(
                deckDefinition.SuitsMaxCount,
                Is.EqualTo(2),
                "should have correct Suits Max Count"
            );

            Assert.That(
                deckDefinition.WildsMaxCount,
                Is.EqualTo(2),
                "should have correct Wilds Max Count"
            );
        }

        [Test, Description("Throws when loading invalid Card Data")]
        public void DeckDefinitionLoader_Load_InvalidCardData()
        {
            var deckPath = "./Assets/DeckDefinitions/invalidCardData.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.Exception,
                "Should throw if card data is invalid"
            );
        }

        [Test, Description("Throws when loading Card Data with duplicated properties")]
        public void DeckDefinitionLoader_Load_InvalidCardData_DuplicatedProperties()
        {
            var deckPath = "./Assets/DeckDefinitions/invalidCardDataDuplicatedProperties.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.Exception,
                "Should throw if card data has duplicated properties"
            );
        }

        [Test, Description("Throws when loading Card Data with extra properties")]
        public void DeckDefinitionLoader_Load_InvalidCardData_ExtraProperties()
        {
            var deckPath = "./Assets/DeckDefinitions/invalidCardDataExtraProperties.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.Exception,
                "Should throw if card data has extra properties"
            );
        }

        [Test, Description("Throws when loading Card Data with wrong properties")]
        public void DeckDefinitionLoader_Load_InvalidCardData_WrongProperties()
        {
            var deckPath = "./Assets/DeckDefinitions/invalidCardDataWrongProperties.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.Exception,
                "Should throw if card data has invalid properties"
            );
        }

        [Test, Description("Throws when loading an invalid Deck Definition Json")]
        public void DeckDefinitionLoader_Load_InvalidDeck()
        {
            var deckPath = "./Assets/DeckDefinitions/invalidDeck.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.Exception,
                "Should throw if deck definition is invalid"
            );
        }

        [Test, Description("Throws when loading an invalid EffectId on Deck Definition Json")]
        public void DeckDefinitionLoader_Load_InvalidEffectId()
        {
            var deckPath = "./Assets/DeckDefinitions/invalidEffectId.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.TypeOf<JsonException>(),
                "Deck definition must have valid Effect Ids"
            );
        }

        [Test, Description("Throws when loading with empty suits in Deck Definition")]
        public void DeckDefinitionLoader_Load_InvalidEmptySuits()
        {
            var deckPath = "./Assets/DeckDefinitions/invalidEmptySuits.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("suitsCount"),
                "suits definition should not be empty"
            );
        }

        [Test, Description("Throws when loading an invalid MaxBaseCopies in Deck Definition")]
        public void DeckDefinitionLoader_Load_InvalidMaxBaseCopies()
        {
            var deckPath = "./Assets/DeckDefinitions/invalidMaxBaseCopies.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("maxBaseCopies"),
                "MaxBaseCopies should be larger than zero"
            );
        }

        [Test, Description("Throws when loading an invalid MaxExtendedCopies in Deck Definition")]
        public void DeckDefinitionLoader_Load_InvalidMaxExtendedCopies()
        {
            var deckPath = "./Assets/DeckDefinitions/invalidMaxExtendedCopies.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("maxExtendedCopies"),
                "MaxExtendedCopies should be larger than zero"
            );
        }

        [Test, Description("Throws when loading an out of range EffectId on Deck Definition Json")]
        public void DeckDefinitionLoader_Load_outOfRangeEffectId()
        {
            var deckPath = "./Assets/DeckDefinitions/outOfRangeEffectId.json";
            Assert.That(File.Exists(deckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(deckPath);
            Assert.That(
                () => DeckDefinitionLoader.Load<DeckDefinition>(jsonString),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("effectId"),
                "Deck definition must have valid Effect Ids"
            );
        }

        [Test, Description("Loads deck definition with no wilds defined")]
        public void DeckDefinitionLoader_Load_ValidDeckEmptyWild()
        {
            var validDeckPath = "./Assets/DeckDefinitions/validDeckEmptyWilds.json";
            Assert.That(File.Exists(validDeckPath), Is.True, "Asset File Exists");
            var jsonString = File.ReadAllText(validDeckPath);
            var deckDefinition = DeckDefinitionLoader.Load<DeckDefinition>(jsonString);

            Assert.That(
                deckDefinition.Suits.Count,
                Is.EqualTo(1),
                "It should have defined one card per suit"
            );

            Assert.That(
                CardDataIsEqual(deckDefinition.Suits[0], new CardData(0, 0, 1, 2)),
                Is.True,
                "It has loaded correctly the card data"
            );

            Assert.That(
                deckDefinition.Wilds.Count,
                Is.EqualTo(0),
                "It should have defined zero wild cards"
            );
        }
    }
}