using TestsPortal.Domain.Questions;

namespace TestsPortal.BL.Services.Questions
{
    public interface IQuestionsService
    {
        IEnumerable<QuestionBase> CreateQuestions(IEnumerable<QuestionBase> questions);
    }
}
