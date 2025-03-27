using NFluent;
using Reservation.Domain.Models;

namespace Reservation.Tests.Domain.Models
{
    public class PersonTests
    {
        [Fact]
        public void Shoud_Create_Person()
        {
            const int id = 100;
            const int idRoom = 100;
            const int idPerson = 100;

            var value = new Person(1,"Fred","Martin");

            Check.That(value.FirstName).Is("Fred");
            Check.That(value.FirstName).Is("Martin");
        }
    }
}
