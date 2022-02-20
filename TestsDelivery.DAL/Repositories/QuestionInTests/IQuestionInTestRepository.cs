using System.Collections.Generic;
using TestsDelivery.DAL.Models.Test;

namespace TestsDelivery.DAL.Repositories.QuestionInTests
{
    public interface IQuestionInTestRepository
    {
        void CreateQuestionsInTests(IEnumerable<QuestionInTest> questions);

        void DeleteQuestionInTests(IEnumerable<long> ids);

        void DeleteQuestionsForTest(long testId);
    }
}
