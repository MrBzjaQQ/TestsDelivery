namespace TestsDelivery.UserModels.Marking.Questions
{
    public record MarkCreateModelBase
    {
        public long QuestionId { get; set; }

        public long TestId { get; set; }

        public string Comment { get; set; }
    }
}
