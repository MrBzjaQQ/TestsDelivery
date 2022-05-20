using TestsPortal.Domain.Candidates;

namespace TestsPortal.Domain.ScheduledTests
{
    public record ScheduledTestInstanceToCreate
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public Candidate Candidate { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public short Status { get; set; }
    }
}
