using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Questions.AnswerOptions
{
    public record AnswerOptionInTestModel
    {
        // TODO: LONG
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Text { get; set; }
    }
}
