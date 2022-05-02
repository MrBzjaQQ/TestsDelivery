using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;

namespace TestsDelivery.UserModels.AnsweredTests
{
    public class AnsweredTestCreateModel
    {
        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }

        public IEnumerable<AnswerReadModelBase> Answers { get; set; }
    }
}
