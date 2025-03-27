using Reservation.Api.Dtos.Requests;
using Reservation.Api.Dtos.Responses;
using Reservation.Domain.Dtos.Services;
using Reservation.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Reservation.Domain.Services;

namespace Reservation.Api.Controllers;
[Route("api/bookings")]
[ApiController]
public class BookingController(IBookingService bookingService, ILogger<BookingController> logger) : ControllerBase
{

    [HttpGet("{id}")]
    [EndpointDescription("Obtenir une booking par son ID")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(PersonResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PersonResponse>> GetBookingByIdAsync(int id)
    {
        logger.Log(LogLevel.Information, "Get booking by ID called with ID: {Id}", id);
        var booking = await bookingService.GetBookingByIdAsync(id);
        if (booking == null)
        {
            return NotFound();
        }
        return Ok(new BookingResponse(booking));
    }

    [HttpPost]
    [EndpointDescription("Créer une réservation")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BookingResponse>> CreateBooking([FromBody] CreateBookingRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        logger.LogInformation("Tentative de création d'une réservation: PersonId {PersonId}, RoomId {RoomId}, Date {BookingDate}, Créneau {StartSlot}-{EndSlot}",
            request.PersonId, request.RoomId, request.BookingDate, request.StartSlot, request.EndSlot);
        try
        {
            var bookingDto = new BookingServiceDto(
                0,
                request.RoomId,
                request.PersonId,
                request.BookingDate,
                request.StartSlot,
                request.EndSlot
            );
            var result = await bookingService.CreateBookingAsync(bookingDto);
            logger.LogInformation("Réservation créée avec succès: ID {BookingId}", result.ReservationId);
            var response = new BookingResponse(
                result.RoomId,
                result.PersonId,
                result.BookingDate,
                result.StartSlot,
                result.EndSlot
            );
            return CreatedAtAction(nameof(CreateBooking), new { id = result.ReservationId }, response);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Erreur de validation lors de la création d'une réservation: {ErrorMessage}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur inattendue lors de la création d'une réservation");
            return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne est survenue lors de la création de la réservation");
        }
    }
    [HttpDelete("{id}")]
    [EndpointDescription("Supprimer une réservation")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        logger.LogInformation("Tentative de suppression de la réservation avec ID {BookingId}", id);

        try
        {
            var result = await bookingService.DeleteBookingAsync(id);

            if (!result)
            {
                logger.LogWarning("Tentative de suppression d'une réservation inexistante: ID {BookingId}", id);
                return NotFound();
            }

            logger.LogInformation("Réservation supprimée avec succès: ID {BookingId}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur inattendue lors de la suppression de la réservation avec ID {BookingId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "Une erreur interne est survenue lors de la suppression de la réservation");
        }
    }
}