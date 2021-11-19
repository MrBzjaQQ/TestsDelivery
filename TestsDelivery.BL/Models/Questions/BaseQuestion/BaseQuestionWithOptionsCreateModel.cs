using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestsDelivery.BL.Models.Questions.AnswerOptions;

namespace TestsDelivery.BL.Models.Questions.BaseQuestion
{
    public record BaseQuestionWithOptionsCreateModel : BaseQuestionCreateModel
    {
        [MinLength(2)]
        public IEnumerable<AnswerOptionCreateModel> AnswerOptions { get; set; }
    }
}
