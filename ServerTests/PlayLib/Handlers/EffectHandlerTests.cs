using CrazyEights.PlayLib.Enums;
using CrazyEights.PlayLib.Handlers;

namespace CrazyEights.Tests.PlayLib.Handlers
{
    [TestFixture]
    public class EffectHandlerTests
    {
        [TestCase(Effects.DrawCards, 5)]
        [TestCase(Effects.InterchangeHands, 10)]
        [TestCase(Effects.ReverseTurnOrder, 15)]
        [TestCase(Effects.SkipTurn, 20)]
        [Description("Correctly initializes EffectHandler and registers and executes effects")]
        public void EffectHandler_Constructor_RegisterExecute(Effects effect, int argValue)
        {
            var control = 0;
            var command = (int arg) => { control = arg; };
            var effectHandler = new EffectHandler();

            Assert.That(
                effectHandler.Register(effect, command),
                Is.True,
                "should return true for a successful registration of an effect command"
            );

            Assert.That(control, Is.EqualTo(0), "should have control variable equal to 0");

            effectHandler.Execute(effect, argValue);

            Assert.That(
                control,
                Is.EqualTo(argValue),
                $"should have executed effect command to set control {control} to arg {argValue}"
            );
        }

        [Test, Description("EffectHandler does nothing with Effect None")]
        public void EffectHandler_RegisterExecute_EffectNone()
        {
            var control = 0;
            var argValue = 7;
            var expectedValue = 0;
            var command = (int arg) => { control = arg; };
            var effectHandler = new EffectHandler();

            Assert.That(
                effectHandler.Register(Effects.None, command),
                Is.False,
                "should return false when trying to register the None effect"
            );

            Assert.That(control, Is.EqualTo(0), "should have control variable equal to 0");

            effectHandler.Execute(Effects.None, argValue);

            Assert.That(
                control,
                Is.EqualTo(expectedValue),
                $"should have not executed any effect command and have control {control} equal to {expectedValue}"
            );
        }
    }
}