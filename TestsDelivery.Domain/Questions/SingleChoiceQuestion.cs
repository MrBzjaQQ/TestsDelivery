using System.Collections.Generic;

namespace TestsDelivery.Domain.Questions
{
    public class SingleChoiceQuestion : QuestionBase
    {
        public IEnumerable<AnswerOption> AnswerOptions { get; set; }
    }
}
