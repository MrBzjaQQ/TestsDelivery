namespace TestsDelivery.UserModels.Marking.Questions
{
    public record ScqMarkCreateModel : MarkCreateModelBase
    {
        public bool IsCorrect { get; set; }
    }
}
