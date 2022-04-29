using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.AnswerOptions
{
    public class AnswerOptionsRepository : BaseRepository<AnswerOption>, IAnswerOptionsRepository
    {
        public AnswerOptionsRepository(TestsPortalContext context)
            : base(context)
        {
        }

        public void CreateAnswerOptions(IEnumerable<AnswerOption> options)
        {
            Context.AnswerOptions.AddRange(options);
            Context.SaveChanges();
        }
    }
}
