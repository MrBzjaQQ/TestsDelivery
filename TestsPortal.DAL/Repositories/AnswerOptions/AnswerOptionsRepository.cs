using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.AnswerOptions
{
    public class AnswerOptionsRepository : IAnswerOptionsRepository
    {
        private readonly TestsPortalContext _context;

        public AnswerOptionsRepository(TestsPortalContext context)
        {
            _context = context;
        }
        public void CreateAnswerOptions(IEnumerable<AnswerOption> options)
        {
            _context.AnswerOptions.AddRange(options);
            _context.SaveChanges();
        }
    }
}
