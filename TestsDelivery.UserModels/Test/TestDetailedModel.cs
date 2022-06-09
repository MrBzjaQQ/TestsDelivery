using TestsDelivery.UserModels.Questions.BaseQuestion;

namespace TestsDelivery.UserModels.Test
{
    public record TestDetailedModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<QuestionDetailedReadModel> Questions { get; set; }
    }
}
