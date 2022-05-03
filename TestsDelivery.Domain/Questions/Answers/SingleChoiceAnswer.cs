namespace TestsDelivery.Domain.Questions.Answers
{
    public record SingleChoiceAnswer : AnswerBase
    {
        public long SelectedAnswerId { get; set; }
    }
}
