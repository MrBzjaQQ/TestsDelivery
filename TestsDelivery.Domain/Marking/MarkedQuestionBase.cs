namespace TestsDelivery.Domain.Marking
{
    public abstract record MarkedQuestionBase
    {
        public long Id { get; set; }

        public long QuestionId { get; set; }

        public long TestInstanceId { get; set; }

        public string Comment { get; set; }
    }
}
