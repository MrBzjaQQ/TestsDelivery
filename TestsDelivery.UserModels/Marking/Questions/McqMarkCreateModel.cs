namespace TestsDelivery.UserModels.Marking.Questions
{
    public record McqMarkCreateModel : MarkCreateModelBase
    {
        // TODO: Delete
        public IEnumerable<long> CorrectAnswerIds { get; set; }
    }
}
