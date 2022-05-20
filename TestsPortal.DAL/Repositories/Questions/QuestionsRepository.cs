using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Repositories.Questions
{
    public class QuestionsRepository : BaseRepository<TestsPortalContext, Question>, IQuestionsRepository
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

        public IEnumerable<ShortQuestion> GetByTestId(long testInstanceId)
        {
            var testId = Context.ScheduledTestInstances
                .Where(x => x.Id == testInstanceId)
                .Include(x => x.ScheduledTest)
                .Include(x => x.ScheduledTest.Test)
                .Select(x => x.ScheduledTest.Test.Id)
                .Single();

            return Mapper.ProjectTo<ShortQuestion>(
                Context.QuestionInTests
                .Where(x => x.TestId == testId)
                .Select(x => x.Question))
                .ToList();
        }

        public override Question GetById(long id)
        {
            return DbSet
                .Include(x => x.Subject)
                .Single(x => x.Id == id);
        }
    }
}
