using CrazyEights.PlayLib.Entities;

namespace CrazyEights.Tests.PlayLib.Entities
{
    [TestFixture]
    public class TeamTests
    {
        [Test, Description("Initializes an empty team")]
        public void Team_Constructor()
        {
            var teamSize = 2;
            var team = new Team(teamSize);

            Assert.That(team.Size, Is.EqualTo(teamSize), $"should have team size {team.Size} equal to {teamSize}");
            Assert.That(
                team.PlayerCount,
                Is.EqualTo(0),
                $"should have team player count {team.PlayerCount} equal to 0"
            );
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Description("Adds players to team")]
        public void Team_AddPlayer_PlayerId(int totalPlayers)
        {
            var teamSize = 3;
            var team = new Team(teamSize);

            for (var i = 0; i < totalPlayers; i++)
            {
                team.AddPlayer(i);
            }

            Assert.That(team.Size, Is.EqualTo(teamSize), $"should have team size {team.Size} equal to {teamSize}");
            Assert.That(
                team.PlayerCount,
                Is.EqualTo(totalPlayers),
                $"should have team player count {team.PlayerCount} equal to {totalPlayers}"
            );
        }

        [Test, Description("Can't add duplicates to the team")]
        public void Team_AddPlayer_DuplicatedId()
        {
            var teamSize = 2;
            var team = new Team(teamSize);

            team.AddPlayer(0);

            Assert.That(
                () => team.AddPlayer(0),
                Throws.TypeOf<InvalidOperationException>().With.Property("Message").EqualTo(
                    "Player id 0 is already in the team"
                ),
                "should throw because player id 0 is already in team"
            );

            Assert.That(team.Size, Is.EqualTo(teamSize), $"should have team size {team.Size} equal to {teamSize}");
            Assert.That(
                team.PlayerCount,
                Is.EqualTo(1),
                $"should have team player count {team.PlayerCount} equal to 1"
            );
        }

        [Test, Description("Can't add to a full team")]
        public void Team_AddPlayer_FullTeam()
        {
            var teamSize = 1;
            var team = new Team(teamSize);

            team.AddPlayer(0);

            Assert.That(
                () => team.AddPlayer(0),
                Throws.TypeOf<InvalidOperationException>().With.Property("Message").EqualTo(
                    "Team of size 1 is full"
                ),
                "should throw if team is full"
            );

            Assert.That(team.Size, Is.EqualTo(teamSize), $"should have team size {team.Size} equal to {teamSize}");
            Assert.That(
                team.PlayerCount,
                Is.EqualTo(1),
                $"should have team player count {team.PlayerCount} equal to 1"
            );
        }

        [Test, Description("Returns true if player is in the team")]
        public void Team_HasPlayer_PlayerIdInTeam()
        {
            var teamSize = 1;
            var playerId = 100;
            var team = new Team(teamSize);

            team.AddPlayer(playerId);
            Assert.That(team.HasPlayer(playerId), Is.True, $"should have player id {playerId}");
        }

        [Test, Description("Returns false if player is not in the team")]
        public void Team_HasPlayer_PlayerIdNotInTeam()
        {
            var teamSize = 1;
            var invalidPlayerId = 100;
            var team = new Team(teamSize);

            team.AddPlayer(0);
            Assert.That(team.HasPlayer(invalidPlayerId), Is.False, $"should not have player id {invalidPlayerId}");
        }

        [Test, Description("Can remove player id from team")]
        public void Team_RemovePlayer_PlayerIdInTeam()
        {
            var teamSize = 1;
            var playerId = 555;
            var team = new Team(teamSize);

            team.AddPlayer(playerId);

            Assert.That(
                team.PlayerCount,
                Is.EqualTo(1),
                $"should have team player count {team.PlayerCount} equal to 1"
            );

            team.RemovePlayer(playerId);

            Assert.That(
                team.PlayerCount,
                Is.EqualTo(0),
                $"should have team player count {team.PlayerCount} equal to 0 after removing player id"
            );
        }

        [Test, Description("Can't remove player id that is not in team")]
        public void Team_RemovePlayer_PlayerIdNotInTeam()
        {
            var teamSize = 1;
            var invalidId = 333;
            var team = new Team(teamSize);

            team.AddPlayer(0);

            Assert.That(
                team.PlayerCount,
                Is.EqualTo(1),
                $"should have team player count {team.PlayerCount} equal to 1"
            );

            Assert.That(
                () => team.RemovePlayer(invalidId),
                Throws.TypeOf<ArgumentOutOfRangeException>().With.Property("ParamName").EqualTo("playerId"),
                $"should throw because player id {invalidId} is not in the team"
            );

            Assert.That(
                team.PlayerCount,
                Is.EqualTo(1),
                $"should still have team player count {team.PlayerCount} equal to 1 after failed removal"
            );
        }
    }
}
