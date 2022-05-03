using System.Collections.Generic;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Questions
{
    public interface IQuestionsRepository : IBaseRepository<Question>
    {
        Question GetQuestion(long id, short questionType);

        IEnumerable<Question> GetQuestionsByTestId(long testId);

        IEnumerable<long> GetQuestionIdsByTestId(long testId);

        void DeleteQuestion(long id, short questionType);
    }
}
