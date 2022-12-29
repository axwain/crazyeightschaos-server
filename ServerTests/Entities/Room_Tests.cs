using CrazyEights.Entities;
using CrazyEights.Exceptions;

namespace CrazyEights.Tests.Entities
{
    [TestFixture]
    public class RoomTests
    {
        [Test, Description("Creates an empty Room with player count cap")]
        public void Room_Creation()
        {
            var maxPlayerCount = 5;
            var room = new Room(maxPlayerCount, 1);
            Assert.That(room.IsEmpty, Is.True, "Room is empty after creation");
            Assert.That(room.MaxPlayers, Is.EqualTo(maxPlayerCount), "Room maximum player count has been set");
            Assert.That(room.Id, Is.EqualTo(1), "Room Id is set");
            Assert.That(room.IsEmpty, Is.True, "Room starts with 0 players");
        }

        [Test, Description("First player added is the room owner")]
        public void Room_Add_Owner()
        {
            var owner = new Player { Id = 0, Name = "Owner" };
            var maxPlayerCount = 5;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
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
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            Assert.That(room.Add(otherPlayer), Is.True, "Room allowed to add player");
            Assert.That(room.PlayerCount, Is.EqualTo(2), "Room should have 2 players");
            Assert.That(room.IsFull, Is.False, "Room should not be full");
        }

        [Test, Description("Changes owner of a Room")]
        public void Room_ChangeOwner()
        {
            var owner = new Player { Id = 0, Name = "First Owner" };
            var otherPlayer = new Player { Id = 1, Name = "Next Owner" };
            var maxPlayerCount = 2;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            room.Add(otherPlayer);
            Assert.That(room.RoomOwner?.Name, Is.EqualTo("First Owner"), "Room owner is first added player");

            room.ChangeRoomOwner(owner.Id, otherPlayer.Id);
            Assert.That(room.RoomOwner?.Name, Is.EqualTo("Next Owner"), "Room owner has changed to the other player");
        }

        [Test, Description("A player can be removed from the room")]
        public void Room_RemovePlayer()
        {
            var owner = new Player { Id = 0, Name = "Owner" };
            var otherPlayer = new Player { Id = 1, Name = "Kicked Player" };
            var maxPlayerCount = 2;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            room.Add(otherPlayer);
            Assert.That(room.PlayerCount, Is.EqualTo(2), "Room has two players");

            room.Remove(otherPlayer.Id);
            Assert.That(room.PlayerCount, Is.EqualTo(1), "Room has one player after removing one");
        }

        [Test, Description("Room fails to create if player limit is invalid")]
        public void Room_Create_InvalidMaxPlayerCount()
        {
            Assert.That(() => new Room(1, 1), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("MaxPlayers"),
                "Room doesn't creates with 1 as MaxPlayers");
            Assert.That(() => new Room(8, 1), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("MaxPlayers"),
                "Room doesn't creates with 8 as MaxPlayers");
            Assert.That(() => new Room(-500, 1), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("MaxPlayers"),
                "Room doesn't create with less than 2 MaxPlayers");
            Assert.That(() => new Room(7853, 1), Throws.TypeOf<ArgumentOutOfRangeException>()
            .With.Property("ParamName").EqualTo("MaxPlayers"),
            "Room doesn't create with more than 8 MaxPlayers");
        }

        [Test, Description("Room fails to create if id is 0 or less")]
        public void Room_Create_InvalidId()
        {
            Assert.That(() => new Room(2, 0), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("Id"),
                "Room doesn't creates with an Id less or equal to zero");
            Assert.That(() => new Room(2, -1), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("Id"),
                "Room doesn't creates with an Id less or equal to zero");
            Assert.That(() => new Room(2, -10000), Throws.TypeOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("Id"),
                "Room doesn't creates with an Id less or equal to zero");
        }

        [Test, Description("Room doesn't allow to add duplicated players")]
        public void Room_Add_Duplicated_Player()
        {
            var owner = new Player { Id = 0, Name = "Owner" };
            var otherPlayer = new Player { Id = 1, Name = "Player1" };
            var maxPlayerCount = 5;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
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
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            room.Add(otherPlayer);
            Assert.That(room.IsFull, Is.True, "Room is full");
            Assert.That(room.Add(excessPlayer), Is.False, "Room didn't allow to add player past the limit");
            Assert.That(room.PlayerCount, Is.EqualTo(2), "Room should have 2 players");
        }

        [Test, Description("Removing a player that isn't in the room does nothing")]
        public void Room_RemovePlayer_IdDoesNotExist()
        {
            var owner = new Player { Id = 0, Name = "Owner" };
            var maxPlayerCount = 2;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            Assert.That(room.PlayerCount, Is.EqualTo(1), "Room should have 1 player before remove");
            Assert.That(room.Remove(4358), Is.False, "Room Remove returned false");
            Assert.That(room.PlayerCount, Is.EqualTo(1), "Room should have 1 player after remove");
        }

        [Test, Description("Removing last player empties the room")]
        public void Room_RemovePlayer_LastPlayer()
        {
            var owner = new Player { Id = 0, Name = "Owner" };
            var maxPlayerCount = 2;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            Assert.That(room.Id, Is.GreaterThan(-1), "Room should have an id");
            Assert.That(room.PlayerCount, Is.EqualTo(1), "Room should have 1 player before remove");
            Assert.That(room.Remove(owner.Id), Is.True, "Room Removed last player");
            Assert.That(room.PlayerCount, Is.EqualTo(0), "Room should have 0 players after remove");
            Assert.That(room.RoomOwner, Is.Null, "Room doesn't have an owner when empty");
        }

        [Test, Description("Removing a player after removing the last one does nothing")]
        public void Room_RemovePlayer_AfterLastOne()
        {
            var owner = new Player { Id = 10, Name = "Owner" };
            var otherPlayer = new Player { Id = 45, Name = "Other Player" };
            var maxPlayerCount = 2;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            room.Add(otherPlayer);
            Assert.That(room.PlayerCount, Is.EqualTo(2), "Room should have 2 players");
            Assert.That(room.Remove(otherPlayer.Id), Is.True, "Room removed other player");
            Assert.That(room.Remove(owner.Id), Is.True, "Room removed last player");
            Assert.That(room.PlayerCount, Is.EqualTo(0), "Room should have 0 players after all the removes");
            Assert.That(
                room.Remove(owner.Id),
                Is.False,
                "Room remove returns false after trying to remove from an empty room"
            );
        }

        [Test, Description("Removing owner changes the owner to another player")]
        public void Room_RemovePlayer_Owner_WhileHavingOtherPlayer()
        {
            var owner = new Player { Id = 10, Name = "Owner" };
            var nextOwner = new Player { Id = 45, Name = "Next Owner" };
            var maxPlayerCount = 2;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            room.Add(nextOwner);
            Assert.That(room.RoomOwner?.Id, Is.EqualTo(owner.Id), "Room should have an owner");
            Assert.That(room.PlayerCount, Is.EqualTo(2), "Room should have 2 players before removing");
            Assert.That(room.Remove(owner.Id), Is.True, "Room removed owner");
            Assert.That(room.RoomOwner?.Id, Is.EqualTo(nextOwner.Id), "Room should have an owner");
            Assert.That(room.PlayerCount, Is.EqualTo(1), "Room should have 1 players after removing");
        }

        [Test, Description("Can't change owner if there is no owner")]
        public void Room_ChangeOwner_WithoutOwner()
        {
            var maxPlayerCount = 2;
            var room = new Room(maxPlayerCount, 1);
            Assert.That(room.RoomOwner, Is.Null, "Room owner isn't set");
            Assert.That(
                () => room.ChangeRoomOwner(1, 2),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo(
                    $"Can't change room owner from {1} to {2}"
                ),
                "Can't change room owner without an owner");
        }

        [Test, Description("Can't change owner of a Room without the owner id")]
        public void Room_ChangeOwner_WithoutValidOwnerId()
        {
            var owner = new Player { Id = 22, Name = "Owner" };
            var otherPlayer = new Player { Id = 33, Name = "Other Owner" };
            var maxPlayerCount = 2;
            var invalidOwnerId = 3;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            room.Add(otherPlayer);
            Assert.That(room.RoomOwner?.Id, Is.EqualTo(owner.Id), "Room owner is set");
            Assert.That(
                () => room.ChangeRoomOwner(invalidOwnerId, otherPlayer.Id),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo(
                    $"Can't change room owner from {invalidOwnerId} to {otherPlayer.Id}"
                ),
                "Can't change room owner with an invalid owner id");
        }

        [Test, Description("Can't change owner of a Room to itself")]
        public void Room_ChangeOwner_ToItself()
        {
            var owner = new Player { Id = 22, Name = "Owner" };
            var otherPlayer = new Player { Id = 33, Name = "Other Owner" };
            var maxPlayerCount = 2;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            room.Add(otherPlayer);
            Assert.That(room.RoomOwner?.Id, Is.EqualTo(owner.Id), "Room owner is set");
            Assert.That(
                () => room.ChangeRoomOwner(owner.Id, owner.Id),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo(
                    $"Can't change room owner from {owner.Id} to {owner.Id}"
                ),
                "Can't change room owner to itself");
        }

        [Test, Description("Can't change owner of a Room to a player not in the room")]
        public void Room_ChangeOwner_ToAPlayerNotInTheRoom()
        {
            var owner = new Player { Id = 22, Name = "Owner" };
            var otherPlayer = new Player { Id = 33, Name = "Other Owner" };
            var maxPlayerCount = 2;
            var invalidPlayerId = 3;
            var room = new Room(maxPlayerCount, 1);
            room.Add(owner);
            room.Add(otherPlayer);
            Assert.That(room.RoomOwner?.Id, Is.EqualTo(owner.Id), "Room owner is set");
            Assert.That(
                () => room.ChangeRoomOwner(owner.Id, invalidPlayerId),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo(
                    $"Can't change room owner from {owner.Id} to {invalidPlayerId}"
                ),
                "Can't make an inexistent player the room owner");
        }
    }
}