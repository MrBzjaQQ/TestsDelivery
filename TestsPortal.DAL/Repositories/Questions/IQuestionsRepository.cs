using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Questions
{
    public interface IQuestionsRepository : IBaseRepository<Models.Questions.Question>
    {
        void CreateQuestions(IEnumerable<Models.Questions.Question> questions);

        IEnumerable<ShortQuestion> GetByTestId(long testId);
    }
}
