using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Reservation.Tests")]

namespace Reservation.Domain.Models;

public record Room(int Id, string RoomName);
