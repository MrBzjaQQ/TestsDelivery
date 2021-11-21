using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.ScheduledTest;

namespace TestsDelivery.DAL.Repositories.ScheduledTest
{
    public class ScheduledTestRepository : IScheduledTestRepository
    {
        private readonly TestsDeliveryContext _context;

        public ScheduledTestRepository(TestsDeliveryContext context)
        {
            _context = context;
        }

        public void ScheduleTest(Models.ScheduledTest.ScheduledTest test)
        {
            _context.ScheduledTests.Add(test);
            _context.SaveChanges();
        }

        public Models.ScheduledTest.ScheduledTest GetTest(long id)
        {
            try
            {
                return _context.ScheduledTests
                    .Include(x => x.Candidate)
                    .Single(test => test.Id == id);
            }
            catch (InvalidOperationException)
            {
                throw new ScheduledTestNotFoundException(id);
            }
        }
    }
}
