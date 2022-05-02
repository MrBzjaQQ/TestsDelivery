using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Answers
{
    public interface ITextAnswersRepository : IBaseRepository<TextAnswer>
    {
        IEnumerable<TextAnswer> GetByTestId(long testId);
    }
}
