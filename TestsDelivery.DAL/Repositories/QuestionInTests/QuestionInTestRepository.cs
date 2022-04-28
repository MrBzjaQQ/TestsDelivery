using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Test;

namespace TestsDelivery.DAL.Repositories.QuestionInTests
{
    public class QuestionInTestRepository : BaseRepository<QuestionInTest>, IQuestionInTestRepository
    {
        public QuestionInTestRepository(TestsDeliveryContext context)
            : base(context)
        {
        }

        public void CreateQuestionsInTests(IEnumerable<QuestionInTest> questions)
        {
            Context.AddRange(questions);
            Context.SaveChanges();
        }

        public void DeleteQuestionsInTests(IEnumerable<long> ids)
        {
            List<QuestionInTest> questions = new();

            foreach (var id in ids)
                questions.Add(new QuestionInTest { Id = id });

            Context.QuestionInTests.AttachRange(questions);
            Context.QuestionInTests.RemoveRange(questions);
            Context.SaveChanges();
        }

        public void DeleteQuestionsForTest(long testId)
        {
            var questions = Context.QuestionInTests.Where(q => q.TestId == testId);
            Context.RemoveRange(questions);
            Context.SaveChanges();
        }
    }
}
