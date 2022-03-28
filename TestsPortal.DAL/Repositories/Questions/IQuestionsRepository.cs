namespace TestsPortal.DAL.Repositories.Questions
{
    public interface IQuestionsRepository
    {
        void CreateQuestions(IEnumerable<Models.Questions.Question> questions);
    }
}
