using System.Linq.Expressions;
using TestsDelivery.DAL.Shared;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.BL.FilterBuilders
{
    public class ScheduledTestsInstancesFilterBuilder : FilterBuilderBase<ScheduledTestInstance>, IScheduledTestsFilterBuilder
    {
        public IScheduledTestsFilterBuilder ByCandidateId(long candidateId)
        {
            Expression<Func<ScheduledTestInstance, bool>> expression = x => x.CandidateId == candidateId;
            And(expression);
            return this;
        }

        public IScheduledTestsFilterBuilder ByKeycode(string keycode)
        {
            Expression<Func<ScheduledTestInstance, bool>> expression = x => x.Keycode == keycode;
            And(expression);
            return this;
        }

        public IScheduledTestsFilterBuilder ByPin(string pin)
        {
            Expression<Func<ScheduledTestInstance, bool>> expression = x => x.Pin == pin;
            And(expression);
            return this;
        }

        public IScheduledTestsFilterBuilder ByTestId(long testId)
        {
            Expression<Func<ScheduledTestInstance, bool>> expression = x => x.Id == testId;
            And(expression);
            return this;
        }
    }
}
