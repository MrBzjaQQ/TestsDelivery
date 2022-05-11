namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record ShortQuestionModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public QuestionType Type { get; set; }
    }
}
