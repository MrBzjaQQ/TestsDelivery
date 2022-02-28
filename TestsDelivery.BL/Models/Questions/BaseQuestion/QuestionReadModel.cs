using System.ComponentModel.DataAnnotations;
using TestsDelivery.BL.Models.Subject;

namespace TestsDelivery.BL.Models.Questions.BaseQuestion
{
    public record QuestionReadModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Text { get; set; }

        public SubjectReadModel Subject { get; set; }
    }
}
