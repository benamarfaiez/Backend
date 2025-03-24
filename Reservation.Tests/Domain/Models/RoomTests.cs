using Reservation.Domain.Models;
using NFluent;

namespace Reservation.Tests.Domain.Models;

public class RoomTests
{
    [Fact]
    public void Room_Constructor_SetsProperties()
    {
        var room = new Room(1, "Test Room");

        Check.That(room.Id).IsEqualTo(1);
        Check.That(room.RoomName).IsEqualTo("Test Room");
    }
}