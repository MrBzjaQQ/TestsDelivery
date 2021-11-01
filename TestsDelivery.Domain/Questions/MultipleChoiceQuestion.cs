using System.Collections.Generic;

namespace TestsDelivery.Domain.Questions
{
    public class MultipleChoiceQuestion : QuestionBase
    {
        public IEnumerable<AnswerOption> AnswerOptions { get; set; }
    }
}
