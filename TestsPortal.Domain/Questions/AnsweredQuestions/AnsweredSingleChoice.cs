namespace TestsPortal.Domain.Questions.AnsweredQuestions
{
    public record AnsweredSingleChoice : AnsweredQuestionBase
    {
        public long SelectedAnswerId { get; set; }
    }
}
