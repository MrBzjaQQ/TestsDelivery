using AutoMapper;
using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Tests;

namespace TestsPortal.DAL.Repositories.Tests
{
    public class TestsRepository : BaseRepository<TestsPortalContext, Test>, ITestsRepository
    {
        public TestsRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
