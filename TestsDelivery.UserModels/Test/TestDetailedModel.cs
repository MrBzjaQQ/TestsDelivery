using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.UserModels.Subject;

namespace TestsDelivery.UserModels.Test
{
    public record TestDetailedModel
    {
        public string Name { get; set; }

        public IEnumerable<QuestionInTestModel> Questions { get; set; }

        public SubjectInTestModel Subject { get; set; }
    }
}
