using System.Collections.Generic;
using TestsDelivery.DAL.Models.Test;

namespace TestsDelivery.DAL.Repositories.QuestionInTests
{
    public interface IQuestionInTestRepository
    {
        void CreateQuestionInTests(IEnumerable<QuestionInTest> questions);

        void DeleteQuestionInTests(IEnumerable<long> ids);

        void DeleteQuestionsForTest(long testId);
    }
}
