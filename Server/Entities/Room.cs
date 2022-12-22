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

        public Room(int maxPlayers)
        {
            Id = -1;
            MaxPlayers = maxPlayers;
            Players = new Dictionary<int, Player>(maxPlayers);
        }

        public void Init(Int32 id, Player owner)
        {
            Id = id;
            Players.Add(owner.Id, owner);
            RoomOwner = owner;
        }

        public bool Add(Player player)
        {
            CheckIfInit();

            if (!IsFull && !Players.ContainsKey(player.Id))
            {
                Players.Add(player.Id, player);
                return true;
            }

            return false;
        }

        public bool Remove(int playerId)
        {
            CheckIfInit();

            if (!IsEmpty && Players.ContainsKey(playerId))
            {
                Players.Remove(playerId);
                if (playerId == RoomOwner?.Id && PlayerCount > 0)
                {
                    RoomOwner = Players.First().Value;
                }

                if (IsEmpty)
                {
                    Id = -1;
                }
                return true;
            }

            return false;
        }

        public void ChangeRoomOwner(int ownerId, int playerId)
        {
            CheckIfInit();
            if (RoomOwner?.Id == ownerId && playerId != ownerId && Players.ContainsKey(playerId))
            {
                RoomOwner = Players[playerId];
            }
            else
            {
                throw new InvalidOperationException($"Can't change room owner from {ownerId} to {playerId}");
            }
        }

        private void CheckIfInit()
        {
            if (Id < 0)
            {
                throw new NotInitializedException("Room Init should be run before any other method");
            }
        }
    }
}