using System.Collections.Generic;
using TestsDelivery.UserModels.AnswerOptions;

namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record QuestionWithOptionsReadModel : QuestionReadModel
    {
        public IEnumerable<AnswerOptionReadModel> AnswerOptions { get; set; }
    }
}
