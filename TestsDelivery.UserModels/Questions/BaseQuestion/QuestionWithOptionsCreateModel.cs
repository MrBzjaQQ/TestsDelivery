using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestsDelivery.UserModels.AnswerOptions;

namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record QuestionWithOptionsCreateModel : QuestionCreateModel
    {
        [MinLength(2)]
        public IEnumerable<AnswerOptionCreateModel> AnswerOptions { get; set; }
    }
}
