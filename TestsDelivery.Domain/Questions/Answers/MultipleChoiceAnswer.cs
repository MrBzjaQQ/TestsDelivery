using System.Collections.Generic;

namespace TestsDelivery.Domain.Questions.Answers
{
    public record MultipleChoiceAnswer : AnswerBase
    {
        public IEnumerable<long> SelectedAnswerIds { get; set; }
    }
}
