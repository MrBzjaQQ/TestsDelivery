using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Tests;

namespace TestsPortal.DAL.Repositories.Tests
{
    public class TestsRepository : ITestsRepository
    {
        private readonly TestsPortalContext _context;

        public TestsRepository(TestsPortalContext context)
        {
            _context = context;
        }

        public void CreateTest(Test test)
        {
            _context.Tests.Add(test);
            _context.SaveChanges();
        }
    }
}
