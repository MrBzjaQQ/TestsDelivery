namespace TestsDelivery.Domain.Questions
{
    public abstract class QuestionBase
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Question { get; set; }

        public Subject.Subject Subject { get; set; }

        public QuestionType Type { get; set; }
    }
}
