namespace TestsPortal.Domain.AnsweredQuestions.Answers
{
    public record MultipleChoiceAnswer
    {
        public IEnumerable<long> SelectedAnswerIds { get; set; }

        public long ScheduledTestId { get; set; }

        public long CandidateId { get; set; }

        public long QuestionId { get; set; }
    }
}
