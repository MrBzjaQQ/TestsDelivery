using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestsDelivery.BL.Models.Questions.AnswerOptions;

namespace TestsDelivery.BL.Models.Questions.BaseQuestion
{
    public record QuestionWithOptionsCreateModel : QuestionCreateModel
    {
        [MinLength(2)]
        public IEnumerable<AnswerOptionCreateModel> AnswerOptions { get; set; }
    }
}
