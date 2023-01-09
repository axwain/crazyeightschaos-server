using CrazyEights.PlayLib.Entities;
using CrazyEights.PlayLib.Enums;

namespace CrazyEights.Tests.PlayLib.Entities
{
    [TestFixture]
    public class RuleSetTests
    {
        [Test, Description("Initializes RuleSet")]
        public void RuleSet_Constructor()
        {
            var ruleSet = new RuleSet();
            Assert.That(ruleSet[RuleIds.AceCardEffect], Is.False, "should start with AceCardEffect false");
            Assert.That(ruleSet[RuleIds.DrawToMatch], Is.False, "should start with DrawToMatch false");
            Assert.That(ruleSet[RuleIds.StackDrawEffect], Is.True, "should start with StackDrawEffect true");
            Assert.That(ruleSet[RuleIds.TurnJump], Is.False, "should start with TurnJump false");
            Assert.That(ruleSet[RuleIds.WCardEffect], Is.False, "should start with WCardEffect false");
            Assert.That(
                ruleSet[RuleIds.WildDrawOverSuitDraw],
                Is.False,
                "should start with WildDrawOverSuitDraw false");
            Assert.That(ruleSet[RuleIds.WildFinish], Is.True, "should start with WildFinish true");
        }

        [TestCase(RuleIds.WildFinish, false, true)]
        [TestCase(RuleIds.StackDrawEffect, false, true)]
        [TestCase(RuleIds.AceCardEffect, true, false)]
        [TestCase(RuleIds.WCardEffect, true, false)]
        [Description("Updates Specific rule to the given value")]
        public void RuleSet_Set_Rule(RuleIds id, bool value, bool previousValue)
        {
            var ruleSet = new RuleSet();
            Assert.That(ruleSet[id], Is.EqualTo(previousValue), $"should have rule {id} set as {previousValue}");
            ruleSet[id] = value;
            Assert.That(ruleSet[id], Is.EqualTo(value), $"should have updated rule {id} to {value}");
        }

        [TestCase(100)]
        [TestCase(-5)]
        [Description("Indexer accessor throws when the rule id is invalid")]
        public void RuleSet_IndexerAccessor_InvalidId(int id)
        {
            var ruleSet = new RuleSet();
            Assert.That(
                () => ruleSet[(RuleIds)id],
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("ruleId"),
                $"should throw with invalid id {id} while trying to read rule");

            Assert.That(
                () => ruleSet[(RuleIds)id] = false,
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("ruleId"),
                $"should throw with invalid id {id} while trying to update rule");
        }
    }
}