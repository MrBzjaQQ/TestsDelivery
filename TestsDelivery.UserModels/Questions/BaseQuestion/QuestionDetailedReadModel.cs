namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record QuestionDetailedReadModel : QuestionReadModel
    {
        public QuestionType Type { get; set; }
    }
}
