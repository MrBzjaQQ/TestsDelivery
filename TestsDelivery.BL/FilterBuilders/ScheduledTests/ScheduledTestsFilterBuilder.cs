using System;
using System.Linq.Expressions;
using TestsDelivery.DAL.Models.ScheduledTest;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.ScheduledTests
{
    public class ScheduledTestsFilterBuilder : FilterBuilderBase<ScheduledTestInList>, IScheduledTestsFilterBuilder
    {
        public IScheduledTestsFilterBuilder ByTestOrCandidateName(string searchCriteria)
        {
            Expression<Func<ScheduledTestInList, bool>> byTestOrCandidateName = x => x.TestName.Contains(searchCriteria)
                || x.Candidate.FirstName.Contains(searchCriteria)
                || x.Candidate.LastName.Contains(searchCriteria)
                || (x.Candidate.FirstName + x.Candidate.LastName).Contains(searchCriteria);
            And(byTestOrCandidateName);
            return this;
        }
    }
}
