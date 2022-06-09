namespace TestsDelivery.UserModels.AnsweredQuestions.AnswerModels
{
    public record SingleChoiceAnswerReadModel : AnswerReadModelBase
    {
        public long SelectedAnswerId { get; set; }
    }
}
