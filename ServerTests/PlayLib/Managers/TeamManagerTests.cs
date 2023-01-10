using System.Diagnostics.Metrics;

using CrazyEights.PlayLib.Managers;
using CrazyEights.Tests.TestUtils.RandomIntMock;

namespace CrazyEights.Tests.PlayLib.Managers
{
    [TestFixture]
    public class TeamManagerTests
    {
        private int[] RawIds { get => new int[] { 123, 456, 789, 210, 543, 896, 777 }; }
        private AbstractRandomIntMock RandomMock { get => new RandomCounterNumberMock(); }

        [TestCase(1, 3, 3)]
        [TestCase(2, 3, 2)]
        [TestCase(2, 4, 2)]
        [TestCase(2, 5, 3)]
        [TestCase(2, 6, 3)]
        [TestCase(3, 4, 2)]
        [TestCase(3, 5, 2)]
        [TestCase(3, 6, 2)]
        [Description("Initializes teams and assigns player ids")]
        public void TeamManager_Constructor(int teamSize, int totalPlayers, int expectedTeamCount)
        {
            var playerIds = new List<int>();
            for (var i = 0; i < totalPlayers; i++)
            {
                playerIds.Add(RawIds[i]);
            }

            var teamManager = new TeamManager(teamSize, playerIds, RandomMock.Next);

            Assert.That(
                teamManager.TeamSize,
                Is.EqualTo(teamSize),
                $"should have team size {teamManager.TeamSize} equal to {teamSize}"
            );

            Assert.That(
                teamManager.TeamCount,
                Is.EqualTo(expectedTeamCount),
                $"should have team count {teamManager.TeamCount} equal to {expectedTeamCount}"
            );

            Assert.That(
                teamManager.PlayerCount,
                Is.EqualTo(playerIds.Count),
                $"should have player count {teamManager.PlayerCount} equal to {playerIds.Count}"
            );

            for (var i = 0; i < playerIds.Count; i++)
            {
                Assert.That(
                    teamManager[i % expectedTeamCount].HasPlayer(playerIds[i]),
                    Is.True,
                    $"should have player id {playerIds[i]} in team id {i}"
                );
            }
        }

        [TestCase(0)]
        [TestCase(4)]
        [Description("Can't initialize Team manager with invalid team size")]
        public void TeamManager_Constructor_InvalidTeamSize(int teamSize)
        {
            var playerIds = new List<int>(new int[] { 345, 555 });

            Assert.That(
                () => new TeamManager(teamSize, playerIds, RandomMock.Next),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("teamSize"),
                $"should throw with invalid team size of {teamSize}"
            );
        }

        [TestCase(2)]
        [TestCase(3)]
        [Description("Can't initialize Team manager with team size equal to team count")]
        public void TeamManager_Constructor_TeamCountEqualsTeamSize(int teamSize)
        {
            var totalPlayers = teamSize;
            var playerIds = new List<int>();
            for (var i = 0; i < totalPlayers; i++)
            {
                playerIds.Add(RawIds[i]);
            }

            Assert.That(
                () => new TeamManager(teamSize, playerIds, RandomMock.Next),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("totalTeams"),
                $"should throw with team size of {teamSize} and total players {totalPlayers}"
            );
        }

        [TestCase(1, 7)]
        [TestCase(2, 6)]
        [TestCase(3, 6)]
        [Description("Can't add player id if teams are full")]
        public void TeamManager_AddPlayer_FullTeam(int teamSize, int totalPlayers)
        {
            var playerIds = new List<int>();
            for (var i = 0; i < totalPlayers; i++)
            {
                playerIds.Add(RawIds[i]);
            }

            var teamManager = new TeamManager(teamSize, playerIds, RandomMock.Next);

            Assert.That(
                () => teamManager.AddPlayer(999),
                Throws.TypeOf<InvalidOperationException>().With.Property("Message").EqualTo("Teams are full"),
                "should throw trying to add player when all teams are full"
            );
        }

        [TestCase(1, 2, 2)]
        [TestCase(1, 3, 3)]
        [TestCase(1, 6, 6)]
        [TestCase(2, 4, 2)]
        [Description("Creates teams to add new players if there's space for more teams")]
        public void TeamManager_AddPlayer_ExtraTeamForPlayer(int teamSize, int totalPlayers, int expectedTeamCount)
        {
            var playerIds = new List<int>();
            for (var i = 0; i < totalPlayers; i++)
            {
                playerIds.Add(RawIds[i]);
            }

            var teamManager = new TeamManager(teamSize, playerIds, RandomMock.Next);

            Assert.That(
                teamManager.TeamCount,
                Is.EqualTo(expectedTeamCount),
                $"should have team count {teamManager.TeamCount} equal to {expectedTeamCount}"
            );

            var afterAddIds = new List<int>(playerIds);
            var additionalId = 999;
            afterAddIds.Add(additionalId);
            teamManager.AddPlayer(additionalId);

            Assert.That(
                teamManager.TeamCount,
                Is.EqualTo(expectedTeamCount + 1),
                $"should have team count {teamManager.TeamCount} equal to {expectedTeamCount + 1}"
            );

            Assert.That(
                teamManager.PlayerCount,
                Is.EqualTo(afterAddIds.Count),
                $"should have player count {teamManager.PlayerCount} equal to {afterAddIds.Count}"
            );

            for (var i = 0; i < playerIds.Count; i++)
            {
                Assert.That(
                    teamManager[i % expectedTeamCount].HasPlayer(playerIds[i]),
                    Is.True,
                    $"should have player id {playerIds[i]} in team id {i}"
                );
            }

            Assert.That(
                teamManager[expectedTeamCount].HasPlayer(additionalId),
                Is.True,
                $"should have player id {additionalId} in team id {expectedTeamCount}"
            );
        }

        [TestCase(2, 3, 2)]
        [TestCase(3, 4, 2)]
        [TestCase(3, 5, 2)]
        [Description("Add player to the team with the least of players")]
        public void TeamManager_AddPlayer_LeastPlayersTeam(int teamSize, int totalPlayers, int expectedTeamCount)
        {
            var playerIds = new List<int>();
            for (var i = 0; i < totalPlayers; i++)
            {
                playerIds.Add(RawIds[i]);
            }

            var teamManager = new TeamManager(teamSize, playerIds, RandomMock.Next);

            Assert.That(
                teamManager.TeamCount,
                Is.EqualTo(expectedTeamCount),
                $"should have team count {teamManager.TeamCount} equal to {expectedTeamCount}"
            );

            var afterAddIds = new List<int>(playerIds);
            var additionalId = 999;
            afterAddIds.Add(additionalId);
            teamManager.AddPlayer(additionalId);

            Assert.That(
                teamManager.PlayerCount,
                Is.EqualTo(afterAddIds.Count),
                $"should have player count {teamManager.PlayerCount} equal to {afterAddIds.Count}"
            );

            for (var i = 0; i < afterAddIds.Count; i++)
            {
                Assert.That(
                    teamManager[i % expectedTeamCount].HasPlayer(afterAddIds[i]),
                    Is.True,
                    $"should have player id {afterAddIds[i]} in team id {i}"
                );
            }
        }

        [TestCase(1, 3, 1, 1, 2)]
        [TestCase(2, 4, 0, 0, 2)]
        [TestCase(2, 4, 3, 1, 2)]
        [TestCase(3, 4, 2, 0, 2)]
        [TestCase(3, 5, 3, 1, 2)]
        [Description("Removes player with valid player id")]
        public void TeamManager_RemovePlayer_ValidId(
            int teamSize,
            int totalPlayers,
            int idIndex,
            int teamId,
            int expectedTeamCount)
        {
            var playerIds = new List<int>();
            for (var i = 0; i < totalPlayers; i++)
            {
                playerIds.Add(RawIds[i]);
            }

            var teamManager = new TeamManager(teamSize, playerIds, RandomMock.Next);

            Assert.That(
                teamManager.PlayerCount,
                Is.EqualTo(totalPlayers),
                $"should have team count {teamManager.TeamCount} equal to {totalPlayers}"
            );

            var afterRemovalIds = new List<int>(playerIds);
            var idToRemove = playerIds[idIndex];
            afterRemovalIds.RemoveAt(idIndex);
            teamManager.RemovePlayer(teamId, idToRemove);

            Assert.That(
                teamManager.TeamCount,
                Is.EqualTo(expectedTeamCount),
                $"should have team count {teamManager.TeamCount} equal to {expectedTeamCount} after removal"
            );

            Assert.That(
                teamManager.PlayerCount,
                Is.EqualTo(totalPlayers - 1),
                $"should have player count {teamManager.TeamCount} equal to {totalPlayers - 1}"
            );
        }

        [TestCase(4, 1)]
        [TestCase(0, 1)]
        [Description("Can't remove player id if either the player id is not in the team or the team id in invalid")]
        public void TeamManager_RemovePlayer_InvalidIds(int teamId, int playerId)
        {
            var playerIds = new List<int>();
            var totalPlayers = 6;
            var teamSize = 2;
            for (var i = 0; i < totalPlayers; i++)
            {
                playerIds.Add(RawIds[i]);
            }

            var teamManager = new TeamManager(teamSize, playerIds, RandomMock.Next);

            Assert.That(
                () => teamManager.RemovePlayer(teamId, playerId),
                Throws.TypeOf<ArgumentOutOfRangeException>(),
                $"should have throw with teamId {teamId} and playerId {playerId}"
            );
        }
    }
}