using System.Collections.Generic;

namespace TestsDelivery.Domain.Questions
{
    public record QuestionWithOptionsBase : QuestionBase
    {
        public IEnumerable<AnswerOption> AnswerOptions { get; set; }
    }
}
