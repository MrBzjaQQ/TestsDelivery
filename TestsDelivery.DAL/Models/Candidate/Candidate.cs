using System.ComponentModel.DataAnnotations;
using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.Candidate
{
    public record Candidate : IdEntity<long>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
