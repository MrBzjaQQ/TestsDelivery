using System.ComponentModel.DataAnnotations;
using TestsDelivery.UserModels.Subject;

namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record QuestionReadModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public SubjectReadModel Subject { get; set; }
    }
}
