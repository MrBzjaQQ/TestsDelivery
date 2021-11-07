using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Questions.AnswerOptions
{
    public record AnswerOptionModelBase
    {
        [Required]
        [MinLength(1)]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
