using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Questions.BaseQuestion
{
    public record BaseQuestionEditModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Text { get; set; }

        [Range(1, long.MaxValue)]
        public long SubjectId { get; set; }
    }
}
