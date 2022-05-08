namespace TestsDelivery.UserModels.Marking.Questions
{
    public record McqMarkCreateModel : MarkCreateModelBase
    {
        public int AmountOfCorrectAnswers { get; set; }
    }
}
