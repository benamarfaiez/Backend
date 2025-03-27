using NFluent;
using Reservation.Domain.Models;

namespace Reservation.Tests.Domain.Models
{
    public class PersonTests
    {
        [Fact]
        public void Shoud_Create_Person()
        {
            var value = new Person(1,"Fred","Martin");

            Check.That(value.FirstName).Is("Fred");
            Check.That(value.LastName).Is("Martin");
        }
    }
}
