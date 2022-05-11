using TestsDelivery.UserModels.Questions.BaseQuestion;

namespace TestsDelivery.UserModels.Questions
{
    public record QuestionsListModel
    {
        public IEnumerable<ShortQuestionModel> Questions { get; set; }

        public int TotalCount { get; set; }
    }
}
