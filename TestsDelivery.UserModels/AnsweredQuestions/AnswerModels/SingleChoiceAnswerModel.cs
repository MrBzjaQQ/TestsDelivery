namespace TestsDelivery.UserModels.AnsweredQuestions.AnswerModels
{
    public record SingleChoiceAnswerModel
    {
        public long SelectedAnswerId { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }
    }
}
