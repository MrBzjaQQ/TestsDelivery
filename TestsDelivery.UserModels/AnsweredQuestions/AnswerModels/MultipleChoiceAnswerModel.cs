namespace TestsDelivery.UserModels.AnsweredQuestions.AnswerModels
{
    public class MultipleChoiceAnswerModel
    {
        public IEnumerable<long> SelectedAnswerIds { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }
    }
}
