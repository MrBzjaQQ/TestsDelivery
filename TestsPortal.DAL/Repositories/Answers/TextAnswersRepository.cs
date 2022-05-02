using AutoMapper;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Answers
{
    public class TextAnswersRepository : BaseRepository<TextAnswer>, ITextAnswersRepository
    {
        public TextAnswersRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public IEnumerable<TextAnswer> GetByTestId(long testId)
        {
            return Context.TextAnswers
                .Where(x => x.ScheduledTestId == testId)
                .ToList();
        }
    }
}
