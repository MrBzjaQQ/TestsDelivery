using TestsDelivery.UserModels.AnswerOptions;

namespace TestsDelivery.UserModels.Questions.BaseQuestion
{
    public record QuestionWithOptionsDetailedReadModel : QuestionDetailedReadModel
    {
        public IEnumerable<AnswerOptionUnverified> AnswerOptions { get; set; }
    }
}
