using Reservation.Domain.Dtos.Repositories;
using Reservation.Domain.Dtos.Services;
using Reservation.Domain.Interfaces.Repositories;
using Reservation.Domain.Services;

namespace Reservation.Tests.Domain.Services;

public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _mockBookingRepository;
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _mockBookingRepository = new Mock<IBookingRepository>();
        _service = new BookingService(_mockBookingRepository.Object);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldCreateBooking_WhenDataIsValid()
    {
        var bookingDto = new BookingServiceDto(
            ReservationId: 0,
            RoomId: 1,
            PersonId: 1,
            BookingDate: DateTime.Today.AddDays(1),
            StartSlot: 9,
            EndSlot: 10
        );

        var expectedResult = new BookingRepositoryDto(
            ReservationId: 1,
            RoomId: bookingDto.RoomId,
            PersonId: bookingDto.PersonId,
            BookingDate: bookingDto.BookingDate,
            StartSlot: bookingDto.StartSlot,
            EndSlot: bookingDto.EndSlot
        );

        _mockBookingRepository
            .Setup(r => r.GetConflictingBookingsAsync(
                bookingDto.RoomId,
                bookingDto.BookingDate,
                bookingDto.StartSlot,
                bookingDto.EndSlot))
            .ReturnsAsync(new List<BookingRepositoryDto>());

        _mockBookingRepository
            .Setup(r => r.CreateBookingAsync(It.IsAny<BookingRepositoryDto>()))
            .ReturnsAsync(expectedResult);

        var result = await _service.CreateBookingAsync(bookingDto);

        Assert.Equal(expectedResult.ReservationId, result.ReservationId);
        Assert.Equal(expectedResult.RoomId, result.RoomId);
        Assert.Equal(expectedResult.PersonId, result.PersonId);
        Assert.Equal(expectedResult.BookingDate, result.BookingDate);
        Assert.Equal(expectedResult.StartSlot, result.StartSlot);
        Assert.Equal(expectedResult.EndSlot, result.EndSlot);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowException_WhenDateIsInPast()
    {
        var bookingDto = new BookingServiceDto(
            ReservationId: 0,
            RoomId: 1,
            PersonId: 1,
            BookingDate: DateTime.Today.AddDays(-1),
            StartSlot: 9,
            EndSlot: 10
        );

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateBookingAsync(bookingDto));
        Assert.Contains("date future", exception.Message);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowException_WhenStartSlotIsAfterEndSlot()
    {
        var bookingDto = new BookingServiceDto(
            ReservationId: 0,
            RoomId: 1,
            PersonId: 1,
            BookingDate: DateTime.Today.AddDays(1),
            StartSlot: 10,
            EndSlot: 9
        );

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateBookingAsync(bookingDto));
        Assert.Contains("heure de début", exception.Message);
    }

    [Fact]
    public async Task CreateBookingAsync_ShouldThrowException_WhenSlotsAreInvalid()
    {
        var bookingDto = new BookingServiceDto(
            ReservationId: 0,
            RoomId: 1,
            PersonId: 1,
            BookingDate: DateTime.Today.AddDays(1),
            StartSlot: 0,
            EndSlot: 25
        );

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateBookingAsync(bookingDto));
        Assert.Contains("créneaux horaires", exception.Message);
    }

    [Fact]
    public async Task DeleteBookingAsync_ShouldReturnTrue_WhenBookingExists()
    {
        var bookingId = 1;
        _mockBookingRepository
            .Setup(r => r.DeleteBookingAsync(bookingId))
            .ReturnsAsync(true);

        var result = await _service.DeleteBookingAsync(bookingId);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteBookingAsync_ShouldReturnFalse_WhenBookingDoesNotExist()
    {
        var bookingId = 1;
        _mockBookingRepository
            .Setup(r => r.DeleteBookingAsync(bookingId))
            .ReturnsAsync(false);

        var result = await _service.DeleteBookingAsync(bookingId);

        Assert.False(result);
    }
}