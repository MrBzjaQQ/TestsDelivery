using System.ComponentModel.DataAnnotations;

namespace TestsPortal.DAL.Models.Candidates
{
    public record Candidate : IdOriginalIdEntity<long>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
    }
}
