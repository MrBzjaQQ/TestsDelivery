using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record QuestionCreateModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        [Range(1, long.MaxValue)]
        public long SubjectId { get; set; }
    }
}
