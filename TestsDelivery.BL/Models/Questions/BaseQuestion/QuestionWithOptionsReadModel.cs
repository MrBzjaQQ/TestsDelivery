using System.Collections.Generic;
using TestsDelivery.BL.Models.Questions.AnswerOptions;

namespace TestsDelivery.BL.Models.Questions.BaseQuestion
{
    public record QuestionWithOptionsReadModel : QuestionReadModel
    {
        public IEnumerable<AnswerOptionReadModel> AnswerOptions { get; set; }
    }
}
