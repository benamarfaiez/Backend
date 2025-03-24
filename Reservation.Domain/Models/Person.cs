using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Reservation.Tests")]

namespace Reservation.Domain.Models;

public record Person(int Id, string FirstName, string LastName);