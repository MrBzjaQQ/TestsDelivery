using TestsDelivery.DAL.Models.ScheduledTest;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.ScheduledTests
{
    public interface IScheduledTestsFilterBuilder : IFilterBuilderBase<ScheduledTestInList>
    {
        IScheduledTestsFilterBuilder ByTestOrCandidateName(string searchCriteria);
    }
}
