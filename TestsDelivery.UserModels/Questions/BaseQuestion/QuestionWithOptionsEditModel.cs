using System.Collections.Generic;
using TestsDelivery.UserModels.AnswerOptions;

namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record QuestionWithOptionsEditModel : QuestionEditModel
    {
        public IEnumerable<AnswerOptionEditModel> AnswerOptions { get; set; }
    }
}
