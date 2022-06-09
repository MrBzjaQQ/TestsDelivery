namespace TestsDelivery.UserModels.Marking.Questions
{
    public abstract record MarkReadModelBase
    {
        public long QuestionId { get; set; }

        public long TestInstanceId { get; set; }

        public string Comment { get; set; }
    }
}
