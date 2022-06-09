using System.Collections.Generic;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Answers
{
    public interface IChoiceAnswersRepository : IBaseRepository<ChoiceAnswer>
    {
        IEnumerable<ChoiceAnswer> GetByTestId(long testId);
    }
}
