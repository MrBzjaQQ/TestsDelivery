using System.Collections.Generic;
using TestsDelivery.Domain.Questions.Answers;

namespace TestsDelivery.Domain.AnsweredTests
{
    public record AnsweredTest
    {
        public long Id { get; set; }

        public long CandidateId { get; set; }

        public string AdminPanelInstance { get; set; }

        public IEnumerable<AnswerBase> AnsweredQuestions { get; set; }
    }
}
