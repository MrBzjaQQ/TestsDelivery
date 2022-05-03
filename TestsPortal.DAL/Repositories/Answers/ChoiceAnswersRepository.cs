using AutoMapper;
using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Answers
{
    public class ChoiceAnswersRepository : BaseRepository<TestsPortalContext, ChoiceAnswer>, IChoiceAnswersRepository
    {
        public ChoiceAnswersRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public IEnumerable<ChoiceAnswer> GetByTestId(long testId)
        {
            return Context.ChoiceAnswers
                .Where(x => x.ScheduledTestId == testId)
                .ToList();
        }
    }
}
