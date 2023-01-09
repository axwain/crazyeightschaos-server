using System;
using System.Collections.Generic;

namespace CrazyEights.PlayLib.Entities
{
    public class Team
    {
        public int Size { get; private set; }
        public int PlayerCount { get => Players.Count; }

        private HashSet<int> Players { get; set; }

        public Team(int size)
        {
            Size = size;
            Players = new HashSet<int>();
        }

        public bool HasPlayer(int id)
        {
            return Players.Contains(id);
        }

        public void AddPlayer(int id)
        {
            if (PlayerCount < Size)
            {
                if (!Players.Add(id))
                {
                    throw new InvalidOperationException($"Player id {id} is already in the team");
                }
            }
            else
            {
                throw new InvalidOperationException($"Team of size {Size} is full");
            }
        }

        public void RemovePlayer(int id)
        {
            if (!Players.Remove(id))
            {
                throw new ArgumentOutOfRangeException("playerId", $"Player id {id} is not in the team");
            }
        }
    }
}