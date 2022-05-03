using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Questions
{
    public interface IQuestionsRepository : IBaseRepository<Question>
    {
        void CreateQuestions(IEnumerable<Models.Questions.Question> questions);

        IEnumerable<ShortQuestion> GetByTestId(long testId);
    }
}
