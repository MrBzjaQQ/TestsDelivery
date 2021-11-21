using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.DAL.Models.Candidate
{
    public record Candidate
    {
        [Key]
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
