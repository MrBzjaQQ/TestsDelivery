using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.AnswerOptions
{
    public record AnswerOptionModelBase
    {
        [Required]
        [MinLength(1)]
        public string Text { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}
