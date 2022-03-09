namespace TestsPortal.Domain.Questions
{
    public record AnswerOption
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public string Text { get; set; }
    }
}
