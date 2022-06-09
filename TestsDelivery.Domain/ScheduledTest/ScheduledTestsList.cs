using System.Collections.Generic;

namespace TestsDelivery.Domain.ScheduledTest
{
    public record ScheduledTestsList
    {
        public IEnumerable<ScheduledTestInListDto> ScheduledTests { get; set; }

        public int TotalCount { get; set; }
    }
}
