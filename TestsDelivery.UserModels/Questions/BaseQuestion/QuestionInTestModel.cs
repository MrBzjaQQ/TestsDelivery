using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record QuestionInTestModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
