using AutoMapper;
using AutoMapper.QueryableExtensions;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Questions
{
    public class QuestionsRepository : BaseRepository<Question>, IQuestionsRepository
    {
        public QuestionsRepository(TestsPortalContext context, IMapper mapper)
            :base(context, mapper)
        {
        }

        public void CreateQuestions(IEnumerable<Question> questions)
        {
            Context.AddRange(questions);
            Context.SaveChanges();
        }

        public IEnumerable<ShortQuestion> GetByTestId(long testId)
        {
            return Mapper.ProjectTo<ShortQuestion>(
                Context.QuestionInTests
                .Where(x => x.TestId == testId)
                .Select(x => x.Question))
                .ToList();
        }
    }
}
