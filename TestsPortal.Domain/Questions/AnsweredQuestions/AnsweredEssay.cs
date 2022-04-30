namespace TestsPortal.Domain.Questions.AnsweredQuestions
{
    public record AnsweredEssay : AnsweredQuestionBase
    {
        public string SelectedAnswer { get; set; }
    }
}
