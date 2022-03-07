using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Candidate
{
    public record CandidateEditModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [MinLength(1)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
