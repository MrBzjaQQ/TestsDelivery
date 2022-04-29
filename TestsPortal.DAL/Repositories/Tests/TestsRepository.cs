using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Tests;

namespace TestsPortal.DAL.Repositories.Tests
{
    public class TestsRepository : BaseRepository<Test>, ITestsRepository
    {
        public TestsRepository(TestsPortalContext context)
            : base(context)
        {
        }
    }
}
