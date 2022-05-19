using TestsPortal.Domain.Candidates;

namespace TestsPortal.Domain.ScheduledTests
{
    public record ScheduledTestInstance
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public Candidate Candidate { get; set; }

        public ScheduledTest ScheduledTest { get; set; }
    }
}
