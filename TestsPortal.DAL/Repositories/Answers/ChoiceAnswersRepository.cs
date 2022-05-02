using AutoMapper;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Answers
{
    public class ChoiceAnswersRepository : BaseRepository<ChoiceAnswer>, IChoiceAnswersRepository
    {
        public ChoiceAnswersRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
