using System.Collections.Generic;
using TestsDelivery.DAL.Models.Test;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.QuestionInTests
{
    public interface IQuestionInTestRepository : IBaseRepository<QuestionInTest>
    {
        void DeleteQuestionsInTests(IEnumerable<long> ids);

        void DeleteQuestionsForTest(long testId);
    }
}
