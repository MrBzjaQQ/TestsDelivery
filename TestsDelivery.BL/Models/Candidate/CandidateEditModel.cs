using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Candidate
{
    public record CandidateEditModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [MinLength(1)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(1)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
