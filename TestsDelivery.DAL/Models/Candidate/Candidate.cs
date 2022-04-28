using System.ComponentModel.DataAnnotations;

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
