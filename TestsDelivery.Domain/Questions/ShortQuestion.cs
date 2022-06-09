namespace TestsDelivery.Domain.Questions
{
    public record ShortQuestion
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public QuestionType Type { get; set; }
    }
}
