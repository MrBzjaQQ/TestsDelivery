using TestsDelivery.DAL.Models.Questions;

namespace TestsDelivery.DAL.Repositories.Questions
{
    public interface IQuestionsRepository
    {
        void CreateQuestion(Question question);

        void EditQuestion(Question question);

        Question GetQuestion(long id);
    }
}
