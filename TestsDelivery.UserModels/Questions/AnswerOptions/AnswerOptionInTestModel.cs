using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.AnswerOptions
{
    public record AnswerOptionInTestModel
    {
        public long Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Text { get; set; } = string.Empty;
    }
}
