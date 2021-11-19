using System.ComponentModel.DataAnnotations;
using TestsDelivery.BL.Models.Subject;

namespace TestsDelivery.BL.Models.Questions.BaseQuestion
{
    public record BaseQuestionReadModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Text { get; set; }

        [Range(1, long.MaxValue)]
        public SubjectReadModel Subject { get; set; }
    }
}
