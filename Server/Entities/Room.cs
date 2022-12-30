using System.Diagnostics;

using CrazyEights.Exceptions;

namespace CrazyEights.Entities
{
    public class Room
    {
        public Int32 Id { get; private set; }
        public Player? RoomOwner { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return Players.Count == 0;
            }
        }

        public bool IsFull
        {
            get
            {
                return Players.Count == MaxPlayers;
            }
        }

        int _maxPlayers;
        public int MaxPlayers
        {
            get => _maxPlayers;

            private set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException("MaxPlayers", "Max Players for a room should not be less than 2");
                }

                if (value > 7)
                {
                    throw new ArgumentOutOfRangeException("MaxPlayers", "Max Players for a room should not be more than 7");
                }

                _maxPlayers = value;
            }
        }

        public int PlayerCount { get { return Players.Count; } }

        private Dictionary<int, Player> Players { get; set; }

        public Room(int maxPlayers, Int32 id)
        {
            if (id < 1)
            {
                throw new ArgumentOutOfRangeException("Id", "Id should be greater than 0");
            }
            Id = id;
            MaxPlayers = maxPlayers;
            Players = new Dictionary<int, Player>(maxPlayers);
        }

        public bool Add(Player player)
        {
            if (!IsFull && !Players.ContainsKey(player.Id))
            {
                if (IsEmpty)
                {
                    RoomOwner = player;
                }
                Players.Add(player.Id, player);
                return true;
            }

            return false;
        }

        public bool Remove(int playerId)
        {
            if (!IsEmpty && Players.ContainsKey(playerId))
            {
                Players.Remove(playerId);
                if (IsEmpty)
                {
                    RoomOwner = null;
                }

                if (playerId == RoomOwner?.Id && !IsEmpty)
                {
                    RoomOwner = Players.First().Value;
                }

                return true;
            }

            return false;
        }

        public void ChangeRoomOwner(int ownerId, int playerId)
        {
            if (RoomOwner?.Id == ownerId && playerId != ownerId && Players.ContainsKey(playerId))
            {
                RoomOwner = Players[playerId];
            }
            else
            {
                throw new InvalidOperationException($"Can't change room owner from {ownerId} to {playerId}");
            }
        }
    }
}