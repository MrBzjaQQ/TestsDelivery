using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Answers
{
    public interface IChoiceAnswersRepository : IBaseRepository<ChoiceAnswer>
    {
        IEnumerable<ChoiceAnswer> GetByTestId(long testId);
    }
}
