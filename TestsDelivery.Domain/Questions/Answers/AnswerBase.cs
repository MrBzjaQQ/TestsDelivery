namespace TestsDelivery.Domain.Questions.Answers
{
    public record AnswerBase
    {
        public long Id { get; set; }

        public long QuestionId { get; set; }
    }
}
