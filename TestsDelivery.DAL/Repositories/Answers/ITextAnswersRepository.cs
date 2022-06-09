using System.Collections.Generic;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Answers
{
    public interface ITextAnswersRepository : IBaseRepository<TextAnswer>
    {
        IEnumerable<TextAnswer> GetByTestId(long testId);
    }
}
