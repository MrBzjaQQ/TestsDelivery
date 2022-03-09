using System.ComponentModel.DataAnnotations;

namespace TestsPortal.DAL.Models.Candidate
{
    public record Candidate
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
    }
}
