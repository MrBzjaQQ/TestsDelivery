using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Test;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.QuestionInTests
{
    public class QuestionInTestRepository : BaseRepository<TestsDeliveryContext, QuestionInTest>, IQuestionInTestRepository
    {
        public QuestionInTestRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
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
