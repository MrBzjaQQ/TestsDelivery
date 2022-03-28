using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.AnswerOptions
{
    public interface IAnswerOptionsRepository
    {
        void CreateAnswerOptions(IEnumerable<AnswerOption> options);
    }
}
