using System.Collections.Generic;
using TestsDelivery.BL.Models.Questions.BaseQuestion;

namespace TestsDelivery.BL.Models.Subject
{
    public record SubjectInTestModel : SubjectBaseModel
    {
        public IEnumerable<QuestionInTestModel> Questions { get; set; }
    }
}
