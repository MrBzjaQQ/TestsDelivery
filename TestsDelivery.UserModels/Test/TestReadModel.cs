using TestsDelivery.UserModels.Questions.BaseQuestion;

namespace TestsDelivery.UserModels.Test
{
    public record TestReadModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<QuestionReadModel> Questions { get; set; }
    }
}
