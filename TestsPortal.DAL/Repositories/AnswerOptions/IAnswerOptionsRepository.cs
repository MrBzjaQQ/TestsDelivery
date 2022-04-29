using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.AnswerOptions
{
    public interface IAnswerOptionsRepository : IBaseRepository<AnswerOption>
    {
        void CreateAnswerOptions(IEnumerable<AnswerOption> options);
    }
}
