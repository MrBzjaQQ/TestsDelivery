namespace TestsPortal.Domain.Questions
{
    public record QuestionWithOptionsBase : QuestionBase
    {
        public IEnumerable<AnswerOption> AnswerOptions { get; set; }
    }
}
