using System.Threading.Tasks;
using TestsDelivery.UserModels.ScheduledTest;

namespace TestsDelivery.BL.Services.Communication
{
    public interface IAdminPanelCommunicationService
    {
        Task ScheduleTest(ScheduledTestDetailedModel test);
    }
}
