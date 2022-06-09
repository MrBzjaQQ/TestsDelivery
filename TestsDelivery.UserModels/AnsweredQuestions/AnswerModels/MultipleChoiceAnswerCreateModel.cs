namespace TestsDelivery.UserModels.AnsweredQuestions.AnswerModels
{
    public class MultipleChoiceAnswerCreateModel
    {
        public IEnumerable<long> SelectedAnswerIds { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }

        public long QuestionId { get; set; }
    }
}
