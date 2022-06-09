using TestsDelivery.UserModels.AnswerOptions;

namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record QuestionWithOptionsInTestModel : QuestionInTestModel
    {
        public IEnumerable<AnswerOptionInTestModel> AnswerOptions { get; set; }
    }
}
