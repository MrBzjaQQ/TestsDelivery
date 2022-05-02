namespace TestsPortal.Domain.Questions.AnsweredQuestions
{
    public record AnsweredQuestionBase
    {
        public long QuestionId { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }
    }
}
