using System;
using System.Collections.Generic;
using System.Linq;

using CrazyEights.PlayLib.Entities;

namespace CrazyEights.PlayLib.Managers
{
    public class TeamManager
    {
        readonly int MAX_TEAMS = 7;
        readonly int MIN_TEAM_SIZE = 1;
        readonly int MAX_TEAM_SIZE = 3;

        public Team this[int key] { get => Teams[key]; }
        public int TeamSize { get; private set; }
        public int PlayerCount
        {
            get => Teams.Aggregate(0, (total, next) => total + next.PlayerCount);
        }

        public IList<int> Players
        {
            get => Teams.Aggregate(
                new List<int>(7),
                (playerIds, next) =>
                {
                    playerIds.AddRange(next.GetPlayerIds());
                    return playerIds;
                }
            );
        }

        public int TeamCount { get => Teams.Count; }

        private IList<Team> Teams { get; set; }

        public TeamManager(int teamSize, IList<int> playerIds, Func<int, int> getRandomInt)
        {
            if (teamSize < MIN_TEAM_SIZE || teamSize > MAX_TEAM_SIZE)
            {
                throw new ArgumentOutOfRangeException(
                    "teamSize",
                    $"teamSize {teamSize} should be between {MIN_TEAM_SIZE} and {MAX_TEAM_SIZE}"
                );
            }

            // ceiling integer division: http://www.cs.nott.ac.uk/~psarb2/G51MPC/slides/NumberLogic.pdf
            var totalTeams = (playerIds.Count + teamSize - 1) / teamSize;
            if (totalTeams <= 1)
            {
                throw new ArgumentOutOfRangeException(
                    "totalTeams",
                    $"team count {totalTeams} shouldn't be less than 2"
                );
            }

            TeamSize = teamSize;
            Teams = new List<Team>(MAX_TEAMS);

            var shuffledPlayerIds = Shuffle(playerIds, getRandomInt);

            AssignTeams(shuffledPlayerIds, totalTeams);
        }

        public void AddPlayer(int playerId)
        {
            if ((TeamSize == 1 && PlayerCount >= 7) ||
                (TeamSize > 1 && PlayerCount >= 6))
            {
                throw new InvalidOperationException("Teams are full");
            }

            if ((TeamSize == 1 && PlayerCount < 7) ||
                (TeamSize == 2 && PlayerCount == 4))
            {
                var team = new Team(TeamSize);
                team.AddPlayer(playerId);
                Teams.Add(team);
            }
            else if (TeamSize > 1)
            {
                var teamIndex = 0;
                var minPlayerCount = MAX_TEAM_SIZE;
                for (var i = 0; i < Teams.Count; i++)
                {
                    if (Teams[i].PlayerCount < minPlayerCount)
                    {
                        teamIndex = i;
                        minPlayerCount = Teams[i].PlayerCount;
                    }
                }
                Teams[teamIndex].AddPlayer(playerId);
            }
        }

        // TODO: Game Manager should end game if only one team remains?
        public void RemovePlayer(int teamId, int playerId)
        {
            Teams[teamId].RemovePlayer(playerId);
            if (Teams[teamId].PlayerCount == 0)
            {
                Teams.RemoveAt(teamId);
            }
        }

        private IList<int> Shuffle(IList<int> playerIds, Func<int, int> getRandomInt)
        {
            var ids = new List<int>(playerIds);
            for (var i = 0; i < ids.Count; i++)
            {
                var index = getRandomInt(ids.Count);
                var temp = ids[index];
                ids[index] = ids[i];
                ids[i] = temp;
            }

            return ids;
        }

        private void AssignTeams(IList<int> playerIds, int totalTeams)
        {
            for (var i = 0; i < totalTeams; i++)
            {
                Teams.Add(new Team(TeamSize));
            }

            var idIndex = 0;
            while (idIndex < playerIds.Count)
            {
                for (var teamIndex = 0; teamIndex < Teams.Count && idIndex < playerIds.Count; teamIndex++, idIndex++)
                {
                    Teams[teamIndex].AddPlayer(playerIds[idIndex]);
                }
            }
        }
    }
}