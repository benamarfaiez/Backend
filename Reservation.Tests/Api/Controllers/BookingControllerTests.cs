﻿using Microsoft.Extensions.Logging;
using Reservation.Domain.Interfaces.Services;
using Reservation.Api.Controllers;
using Reservation.Api.Dtos.Requests;
using Reservation.Api.Dtos.Responses;
using Reservation.Domain.Dtos.Services;
using Microsoft.AspNetCore.Mvc;
using Reservation.Domain.Models;

namespace Reservation.Tests.Api.Controllers;

public class BookingControllerTests
{
    private readonly Mock<IBookingService> _mockBookingService;
    private readonly Mock<ILogger<BookingController>> _mockLogger;
    private readonly BookingController _controller;

    public BookingControllerTests()
    {
        _mockBookingService = new Mock<IBookingService>();
        _mockLogger = new Mock<ILogger<BookingController>>();
        _controller = new BookingController(_mockBookingService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateBooking_ShouldReturnCreatedAtAction_WhenBookingIsValid()
    {
        var request = new CreateBookingRequest(
            RoomId: 1,
            PersonId: 1,
            BookingDate: DateTime.Today.AddDays(1),
            StartSlot: 9,
            EndSlot: 10
        );

        var expectedResult = new BookingServiceDto(
            ReservationId: 1,
            RoomId: request.RoomId,
            PersonId: request.PersonId,
            BookingDate: request.BookingDate,
            StartSlot: request.StartSlot,
            EndSlot: request.EndSlot
        );

        _mockBookingService
            .Setup(s => s.CreateBookingAsync(It.IsAny<BookingServiceDto>()))
            .ReturnsAsync(expectedResult);

        var result = await _controller.CreateBooking(request);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<BookingResponse>(createdAtActionResult.Value);
        Assert.Equal(expectedResult.RoomId, returnValue.RoomId);
        Assert.Equal(expectedResult.PersonId, returnValue.PersonId);
        Assert.Equal(expectedResult.BookingDate, returnValue.BookingDate);
        Assert.Equal(expectedResult.StartSlot, returnValue.StartSlot);
        Assert.Equal(expectedResult.EndSlot, returnValue.EndSlot);
    }

    [Fact]
    public async Task CreateBooking_ShouldReturnBadRequest_WhenServiceThrowsException()
    {
        var request = new CreateBookingRequest(
            RoomId: 1,
            PersonId: 1,
            BookingDate: DateTime.Today.AddDays(1),
            StartSlot: 9,
            EndSlot: 10
        );

        var exceptionMessage = "La salle est déjà réservée";
        _mockBookingService
            .Setup(s => s.CreateBookingAsync(It.IsAny<BookingServiceDto>()))
            .ThrowsAsync(new InvalidOperationException(exceptionMessage));

        var result = await _controller.CreateBooking(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(exceptionMessage, badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnNoContent_WhenBookingExists()
    {
        var bookingId = 1;
        _mockBookingService
            .Setup(s => s.DeleteBookingAsync(bookingId))
            .ReturnsAsync(true);

        var result = await _controller.DeleteBooking(bookingId);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnNotFound_WhenBookingDoesNotExist()
    {
        var bookingId = 1;
        _mockBookingService
            .Setup(s => s.DeleteBookingAsync(bookingId))
            .ReturnsAsync(false);

        var result = await _controller.DeleteBooking(bookingId);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnBookingAtAction_WhenBookingIsValid()
    {
        var reservationId = 1;
        var bookingDto = new BookingServiceDto(
            new Booking(
                ReservationId: 1,
                RoomId: 5,
                PersonId: 8,
                BookingDate: DateTime.Now,
                StartSlot: 10,
                EndSlot: 12
            )
        );


        _mockBookingService.Setup(service => service.GetBookingByIdAsync(reservationId))
            .ReturnsAsync(bookingDto);

        var result = await _controller.GetBookingByIdAsync(reservationId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<BookingResponse>(okResult.Value);

        Assert.Equal(bookingDto.RoomId, response.RoomId);
        Assert.Equal(bookingDto.PersonId, response.PersonId);
        Assert.Equal(bookingDto.BookingDate, response.BookingDate);
        Assert.Equal(bookingDto.StartSlot, response.StartSlot);
        Assert.Equal(bookingDto.EndSlot, response.EndSlot);
    }
}