namespace Reservation.Dal.Entities;

public partial class RoomEntity
{
    public int Id { get; set; }

    public string RoomName { get; set; } = null!;

    public virtual ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
}
