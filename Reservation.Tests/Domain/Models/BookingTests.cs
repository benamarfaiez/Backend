using NFluent;
using Reservation.Domain.Models;

namespace Reservation.Tests.Domain.Models;

public class BookingTests
{
    [Fact]
    public void Shoud_Create_Booking()
    {
        const int id = 100;
        const int idRoom = 100;
        const int idPerson = 100;

        var value = new Booking(id, idRoom, idPerson,DateTime.Now,1,1);

        Check.That(value.ReservationId).Is(id);
        Check.That(value.RoomId).Is(idRoom);
    }
}