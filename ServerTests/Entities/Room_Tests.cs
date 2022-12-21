using CrazyEights.Entities;

namespace CrazyEights.Tests.Entities
{
    [TestFixture]
    public class RoomTests
    {
        [Test, Description("Creates an empty Room with player count cap")]
        public void Room_Creation()
        {
            var maxPlayerCount = 5;
            var room = new Room(maxPlayerCount);
            Assert.That(room.IsEmpty, Is.True, "Room is empty after creation");
            Assert.That(room.MaxPlayers, Is.EqualTo(maxPlayerCount), "Room maximum player count has been set");
            Assert.That(room.Id, Is.EqualTo(-1), "Room Id is not set after creation");
        }

        [Test, Description("Initializes an empty Room with owner and id")]
        public void Room_Init()
        {
            var owner = new Player { Id = 0, Name = "Owner" };
            var maxPlayerCount = 5;
            var room = new Room(maxPlayerCount);
            room.Init(1, owner);
            Assert.That(room.IsEmpty, Is.False, "Room is not empty after init");
            Assert.That(room.PlayerCount, Is.EqualTo(1), "Room should have 1 player");
            Assert.That(room.RoomOwner?.Id, Is.EqualTo(owner.Id), "Room has an owner set");
            Assert.That(room.Id, Is.EqualTo(1), "Room has an Id set");
        }

        [Test, Description("Adds a player to a Room")]
        public void Room_Add_Player()
        {
            var owner = new Player { Id = 0, Name = "Owner" };
            var otherPlayer = new Player { Id = 1, Name = "Player1" };
            var maxPlayerCount = 5;
            var room = new Room(maxPlayerCount);
            room.Init(1, owner);
            Assert.That(room.Add(otherPlayer), Is.True, "Room allowed to add player");
            Assert.That(room.PlayerCount, Is.EqualTo(2), "Room should have 2 players");
            Assert.That(room.IsFull, Is.False, "Room should not be full");
        }

        [Test, Description("Room doesn't allow to add duplicated players")]
        public void Room_Add_Duplicated_Player()
        {
            var owner = new Player { Id = 0, Name = "Owner" };
            var otherPlayer = new Player { Id = 1, Name = "Player1" };
            var maxPlayerCount = 5;
            var room = new Room(maxPlayerCount);
            room.Init(1, owner);
            room.Add(otherPlayer);
            Assert.That(room.Add(otherPlayer), Is.False, "Room didn't allow to add player");
            Assert.That(room.PlayerCount, Is.EqualTo(2), "Room should have 2 players");
        }

        [Test, Description("Room doesn't allow to add players past the limit")]
        public void Room_Add_Player_Past_Limit()
        {
            var owner = new Player { Id = 0, Name = "Owner" };
            var otherPlayer = new Player { Id = 1, Name = "Player1" };
            var excessPlayer = new Player { Id = 2, Name = "Player2" };
            var maxPlayerCount = 2;
            var room = new Room(maxPlayerCount);
            room.Init(1, owner);
            room.Add(otherPlayer);
            Assert.That(room.IsFull, Is.True, "Room is full");
            Assert.That(room.Add(excessPlayer), Is.False, "Room didn't allow to add player past the limit");
            Assert.That(room.PlayerCount, Is.EqualTo(2), "Room should have 2 players");
        }

        [Test, Description("Room fails to create if player limit is invalid")]
        public void Room_Create_InvalidMaxPlayerCount()
        {
            Assert.That(() => new Room(1), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("MaxPlayers"),
                "Room doesn't creates with 1 as MaxPlayers");
            Assert.That(() => new Room(8), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("MaxPlayers"),
                "Room doesn't creates with 8 as MaxPlayers");
            Assert.That(() => new Room(-500), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("MaxPlayers"),
                "Room doesn't create with less than 2 MaxPlayers");
            Assert.That(() => new Room(7853), Throws.TypeOf<ArgumentOutOfRangeException>()
            .With.Property("ParamName").EqualTo("MaxPlayers"),
            "Room doesn't create with more than 8 MaxPlayers");
        }
    }
}