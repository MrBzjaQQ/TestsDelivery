namespace TestsPortal.Domain.AnsweredQuestions.Answers
{
    public record SingleChoiceAnswer
    {
        public long SelectedAnswerId { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }
    }
}
