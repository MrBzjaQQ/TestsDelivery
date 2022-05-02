using TestsPortal.Domain.Questions;
using TestsPortal.Domain.Questions.AnsweredQuestions;

namespace TestsPortal.Domain.AnsweredTests
{
    public record AnsweredTest
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public long CandidateId { get; set; }

        public string AdminPanelInstance { get; set; }

        public IEnumerable<AnsweredQuestionBase> AnsweredQuestions { get; set; }
    }
}
