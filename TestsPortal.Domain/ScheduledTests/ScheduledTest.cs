using TestsPortal.Domain.Candidates;

namespace TestsPortal.Domain.ScheduledTests
{
    public record ScheduledTest
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public Tests.Test Test { get; set; }

        public IEnumerable<Candidate> Candidates { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public TestStatus Status { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
