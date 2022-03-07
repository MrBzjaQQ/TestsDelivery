using TestsDelivery.UserModels.Questions.BaseQuestion;

namespace TestsDelivery.UserModels.Subject
{
    public record SubjectInTestModel : SubjectBaseModel
    {
        public IEnumerable<QuestionInTestModel> Questions { get; set; }
    }
}
