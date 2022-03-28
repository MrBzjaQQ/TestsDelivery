using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Questions
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly TestsPortalContext _context;

        public QuestionsRepository(TestsPortalContext context)
        {
            _context = context;
        }

        public void CreateQuestions(IEnumerable<Question> questions)
        {
            _context.AddRange(questions);
            _context.SaveChanges();
        }
    }
}
