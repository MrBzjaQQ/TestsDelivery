using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Questions.BaseQuestion
{
    public record QuestionInTestModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
