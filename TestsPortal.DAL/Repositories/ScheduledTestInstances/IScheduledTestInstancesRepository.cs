using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Repositories.ScheduledTestInstances
{
    public interface IScheduledTestInstancesRepository : IBaseRepository<ScheduledTestInstance>
    {
        string GetAdminInstanceForTest(long testId);
    }
}
