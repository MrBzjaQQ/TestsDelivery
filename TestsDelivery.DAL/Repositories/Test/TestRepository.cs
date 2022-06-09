using AutoMapper;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Shared.Repository;
using TestData = TestsDelivery.DAL.Models.Test.Test;

namespace TestsDelivery.DAL.Repositories.Test
{
    public class TestRepository : BaseRepository<TestsDeliveryContext, TestData>, ITestRepository
    {
        public TestRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
