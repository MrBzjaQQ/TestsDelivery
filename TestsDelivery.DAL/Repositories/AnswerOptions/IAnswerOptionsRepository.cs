using System.Collections.Generic;
using TestsDelivery.DAL.Models.Questions;

namespace TestsDelivery.DAL.Repositories.AnswerOptions
{
    public interface IAnswerOptionsRepository
    {
        void CreateAnswerOption(AnswerOption answerOption);

        void CreateAnswerOptions(IEnumerable<AnswerOption> answerOptions);

        void EditAnswerOption(AnswerOption answerOption);

        void EditAnswerOptions(IEnumerable<AnswerOption> answerOptions);

        AnswerOption GetAnswerOption(long id);

        IEnumerable<AnswerOption> GetAnswerOptionsForQuestion(long questionId);
    }
}
