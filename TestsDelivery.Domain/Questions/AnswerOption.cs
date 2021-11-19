namespace TestsDelivery.Domain.Questions
{
    public record AnswerOption
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
