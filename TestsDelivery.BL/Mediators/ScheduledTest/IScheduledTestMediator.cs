using System.Threading.Tasks;
using TestsDelivery.UserModels.Lists;
using TestsDelivery.UserModels.ScheduledTest;

namespace TestsDelivery.BL.Mediators.ScheduledTest
{
    public interface IScheduledTestMediator
    {
        Task<ScheduledTestReadModel> ScheduleTest(ScheduledTestCreateModel model);

        ScheduledTestReadModel GetTest(long id);

        ScheduledTestsListModel GetList(ListModel model);
    }
}
