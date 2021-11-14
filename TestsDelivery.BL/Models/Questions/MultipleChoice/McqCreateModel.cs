using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestsDelivery.BL.Models.Questions.AnswerOptions;

namespace TestsDelivery.BL.Models.Questions.MultipleChoice
{
    public record McqCreateModel
    {
        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        public string Text { get; set; }

        [Range(1, int.MaxValue)]
        public int SubjectId { get; set; }

        [MinLength(2)]
        public IEnumerable<AnswerOptionCreateModel> AnswerOptions { get; set; }
    }
}
