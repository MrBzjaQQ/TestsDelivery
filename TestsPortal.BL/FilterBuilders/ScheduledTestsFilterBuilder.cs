using System.Linq.Expressions;
using TestsDelivery.DAL.Shared;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.BL.FilterBuilders
{
    public class ScheduledTestsFilterBuilder : FilterBuilderBase<ScheduledTest>, IScheduledTestsFilterBuilder
    {
        public IScheduledTestsFilterBuilder ByTestId(long testId)
        {
            Expression<Func<ScheduledTest, bool>> expression = x => x.Id == testId;
            And(expression);
            return this;
        }
    }
}
