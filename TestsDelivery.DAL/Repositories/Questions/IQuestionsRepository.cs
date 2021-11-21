using System.Collections.Generic;
using TestsDelivery.DAL.Models.Questions;

namespace TestsDelivery.DAL.Repositories.Questions
{
    public interface IQuestionsRepository
    {
        void CreateQuestion(Question question);

        void EditQuestion(Question question);

        Question GetQuestion(long id);

        Question GetQuestion(long id, short questionType);

        IEnumerable<Question> GetQuestionsByTestId(long testId);

        IEnumerable<long> GetQuestionIdsByTestId(long testId);
    }
}
