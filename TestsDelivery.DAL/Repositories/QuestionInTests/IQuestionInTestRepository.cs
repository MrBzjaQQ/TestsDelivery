using System.Collections.Generic;
using TestsDelivery.DAL.Models.Test;

namespace TestsDelivery.DAL.Repositories.QuestionInTests
{
    public interface IQuestionInTestRepository : IBaseRepository<QuestionInTest>
    {
        void CreateQuestionsInTests(IEnumerable<QuestionInTest> questions);

        void DeleteQuestionsInTests(IEnumerable<long> ids);

        void DeleteQuestionsForTest(long testId);
    }
}
