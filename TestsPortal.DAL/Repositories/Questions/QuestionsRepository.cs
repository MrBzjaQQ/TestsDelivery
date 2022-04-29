using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Questions
{
    public class QuestionsRepository : BaseRepository<Question>, IQuestionsRepository
    {
        public QuestionsRepository(TestsPortalContext context)
            :base(context)
        {
        }

        public void CreateQuestions(IEnumerable<Question> questions)
        {
            Context.AddRange(questions);
            Context.SaveChanges();
        }
    }
}
