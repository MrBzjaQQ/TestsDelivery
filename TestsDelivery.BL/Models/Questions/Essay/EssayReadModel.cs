using System.ComponentModel.DataAnnotations;
using TestsDelivery.BL.Models.Subject;

namespace TestsDelivery.BL.Models.Questions.Essay
{
    public record EssayReadModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Question { get; set; }

        [Range(1, long.MaxValue)]
        public SubjectReadModel Subject{ get; set; }
    }
}
