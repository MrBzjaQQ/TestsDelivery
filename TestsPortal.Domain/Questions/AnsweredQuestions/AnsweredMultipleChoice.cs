namespace TestsPortal.Domain.Questions.AnsweredQuestions
{
    public record AnsweredMultipleChoice : AnsweredQuestionBase
    {
        public IEnumerable<long> SelectedAnswerIds { get; set; }
    }
}
