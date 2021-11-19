using System.Collections.Generic;
using TestsDelivery.BL.Models.Questions.AnswerOptions;

namespace TestsDelivery.BL.Models.Questions.BaseQuestion
{
    public record BaseQuestionWithOptionsEditModel : BaseQuestionEditModel
    {
        public IEnumerable<AnswerOptionEditModel> AnswerOptions { get; set; }
    }
}
