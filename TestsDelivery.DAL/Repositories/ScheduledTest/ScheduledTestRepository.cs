using AutoMapper;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.ScheduledTest
{
    public class ScheduledTestRepository : BaseRepository<TestsDeliveryContext, Models.ScheduledTest.ScheduledTest>, IScheduledTestRepository
    {
        public ScheduledTestRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
