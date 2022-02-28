using System.Collections.Generic;
using TestsDelivery.BL.Models.Questions.AnswerOptions;

namespace TestsDelivery.BL.Models.Questions.BaseQuestion
{
    public record QuestionWithOptionsInTestModel : QuestionInTestModel
    {
        public IEnumerable<AnswerOptionInTestModel> AnswerOptions { get; set; }
    }
}
