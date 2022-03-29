using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Repositories.ScheduledTests
{
    public class ScheduledTestsRepository : IScheduledTestsRepository
    {
        private readonly TestsPortalContext _context;

        public ScheduledTestsRepository(TestsPortalContext context)
        {
            _context = context;
        }

        public void CreateScheduledTest(ScheduledTest scheduledTest)
        {
            _context.ScheduledTests.Add(scheduledTest);
            _context.SaveChanges();
        }
    }
}
