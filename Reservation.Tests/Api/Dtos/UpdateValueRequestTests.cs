using System.ComponentModel.DataAnnotations;
using NFluent;
using Reservation.Api.Dtos.Requests;

namespace Reservation.Tests.Api.Dtos;

public class UpdateValueRequestTests
{
    [Fact]
    public void Should_Valid_Request()
    {
        var request = CreateValidRequest();
        var validationContext = new ValidationContext(request);
        var errors = new List<ValidationResult>();

        var result = Validator.TryValidateObject(request, validationContext, errors, true);

        Check.That(result).IsTrue();
        Check.That(errors).HasSize(0);
    }

    private static UpdateBookingRequest CreateValidRequest() => new(11,1,DateTime.Now,2,1);
}