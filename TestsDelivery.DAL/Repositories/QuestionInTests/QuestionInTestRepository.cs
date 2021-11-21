using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Test;

namespace TestsDelivery.DAL.Repositories.QuestionInTests
{
    public class QuestionInTestRepository : IQuestionInTestRepository
    {
        private readonly TestsDeliveryContext _context;

        public QuestionInTestRepository(TestsDeliveryContext context)
        {
            _context = context;
        }

        public void CreateQuestionInTests(IEnumerable<QuestionInTest> questions)
        {
            _context.AddRange(questions);
            _context.SaveChanges();
        }

        public void DeleteQuestionInTests(IEnumerable<long> ids)
        {
            List<QuestionInTest> questions = new();

            foreach (var id in ids)
                questions.Add(new QuestionInTest { Id = id });

            _context.QuestionInTests.AttachRange(questions);
            _context.QuestionInTests.RemoveRange(questions);
            _context.SaveChanges();
        }

        public void DeleteQuestionsForTest(long testId)
        {
            var questions = _context.QuestionInTests.Where(q => q.TestId == testId);
            _context.RemoveRange(questions);
            _context.SaveChanges();
        }
    }
}
