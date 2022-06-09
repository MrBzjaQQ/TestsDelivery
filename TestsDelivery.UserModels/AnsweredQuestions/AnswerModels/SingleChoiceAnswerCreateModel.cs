namespace TestsDelivery.UserModels.AnsweredQuestions.AnswerModels
{
    public record SingleChoiceAnswerCreateModel
    {
        public long SelectedAnswerId { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }

        public long QuestionId { get; set; }
    }
}
