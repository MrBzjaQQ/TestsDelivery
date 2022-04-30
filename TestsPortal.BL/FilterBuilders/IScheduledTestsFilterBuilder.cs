using TestsDelivery.DAL.Shared;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.BL.FilterBuilders
{
    public interface IScheduledTestsFilterBuilder : IFilterBuilderBase<ScheduledTest>
    {
        IScheduledTestsFilterBuilder ByTestId(long testId);

        IScheduledTestsFilterBuilder ByKeycode(string keycode);

        IScheduledTestsFilterBuilder ByPin(string pin);
    }
}
