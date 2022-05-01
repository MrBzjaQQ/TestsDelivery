namespace TestsPortal.Domain.AnsweredQuestions.Answers
{
    public record EssayAnswer
    {
        public string Text { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }
    }
}
