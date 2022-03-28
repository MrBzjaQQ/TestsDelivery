using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.UserModels.Subject;

namespace TestsDelivery.UserModels.Test
{
    public record TestDetailedModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<QuestionReadModel> Questions { get; set; }
    }
}
