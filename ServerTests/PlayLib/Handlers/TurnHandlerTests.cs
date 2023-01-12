using CrazyEights.PlayLib.Handlers;

namespace CrazyEights.Tests.PlayLib.Handlers
{
    [TestFixture]
    public class TurnHandlerTests
    {
        private IList<int> PlayerIdsMock { get; } = new List<int>(new int[] { 111, 222, 333, 444, 555 });
        private Func<int, int> RandomIntMock { get => (_) => 2; }

        [Test, Description("Initializes TurnHandler")]
        public void TurnHandler_Constructor()
        {
            var turnHandler = new TurnHandler(PlayerIdsMock, RandomIntMock);
            Assert.That(turnHandler.NextId, Is.EqualTo(333), "should have initialized TurnHandler on the third player");
        }

        [Test, Description("TurnHandler gives the correct next turn")]
        public void TurnHandler_ComputeNextTurn_FirstTurn()
        {
            var turnHandler = new TurnHandler(PlayerIdsMock, RandomIntMock);
            turnHandler.ComputeNextTurn();
            Assert.That(turnHandler.NextId, Is.EqualTo(444), "should have the fourth player as the next one");
        }

        [Test, Description("TurnHandler gives the correct next turn after looping around players")]
        public void TurnHandler_ComputeNextTurn_LoopAround()
        {
            var turnHandler = new TurnHandler(PlayerIdsMock, RandomIntMock);
            for (var i = 0; i < 3; i++)
            {
                turnHandler.ComputeNextTurn();
            }
            Assert.That(turnHandler.NextId, Is.EqualTo(111), "should have the first player as the next one");
        }

        [Test, Description("TurnHandler reverses turn order")]
        public void TurnHandler_ReverseOrder()
        {
            var turnHandler = new TurnHandler(PlayerIdsMock, RandomIntMock);
            turnHandler.ReverseOrder();
            turnHandler.ComputeNextTurn();
            Assert.That(turnHandler.NextId, Is.EqualTo(222), "should have the second player as the next one");
        }

        [Test, Description("TurnHandler gives the correct next turn after looping around players in reverse order")]
        public void TurnHandler_ComputeNextTurn_ReverseLoopAround()
        {
            var turnHandler = new TurnHandler(PlayerIdsMock, RandomIntMock);
            turnHandler.ReverseOrder();
            for (var i = 0; i < 3; i++)
            {
                turnHandler.ComputeNextTurn();
            }
            Assert.That(turnHandler.NextId, Is.EqualTo(555), "should have the fifth player as the next one");
        }

        [TestCase(1, 555)]
        [TestCase(2, 111)]
        [TestCase(8, 222)]
        [TestCase(0, 333)]
        [TestCase(-1, 111)]
        [TestCase(-2, 555)]
        [TestCase(-8, 444)]
        [Description("TurnHandler skips turns")]
        public void TurnHandler_SkipTurns(int totalSkips, int expectedId)
        {
            var turnHandler = new TurnHandler(PlayerIdsMock, RandomIntMock);
            turnHandler.SkipTurns(totalSkips);
            turnHandler.ComputeNextTurn();
            Assert.That(
                turnHandler.NextId,
                Is.EqualTo(expectedId),
                $"should have skipped {totalSkips} turn(s) to playerId {expectedId}"
            );
        }

        [TestCase(1, 111)]
        [TestCase(2, 555)]
        [TestCase(8, 444)]
        [TestCase(0, 333)]
        [TestCase(-1, 555)]
        [TestCase(-2, 111)]
        [TestCase(-8, 222)]
        [Description("TurnHandler skips turns with reverse order")]
        public void TurnHandler_SkipTurns_ReverseOrder(int totalSkips, int expectedId)
        {
            var turnHandler = new TurnHandler(PlayerIdsMock, RandomIntMock);
            turnHandler.ReverseOrder();
            turnHandler.SkipTurns(totalSkips);
            turnHandler.ComputeNextTurn();
            Assert.That(
                turnHandler.NextId,
                Is.EqualTo(expectedId),
                $"should have skipped {totalSkips} turn(s) to playerId {expectedId} in reverse order"
            );
        }
    }
}