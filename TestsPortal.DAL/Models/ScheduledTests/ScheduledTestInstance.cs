using System.ComponentModel.DataAnnotations.Schema;
using TestsPortal.DAL.Models.Candidates;

namespace TestsPortal.DAL.Models.ScheduledTests
{
    public record ScheduledTestInstance : IdOriginalIdEntity<long>
    {
        [ForeignKey(nameof(Candidate))]
        public long CandidateId { get; set; }

        [ForeignKey(nameof(ScheduledTests.ScheduledTest))]
        public long ScheduledTestId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual ScheduledTest ScheduledTest { get; set; }

        public string Keycode { get; set; } = string.Empty;

        public string Pin { get; set; } = string.Empty;

        public short Status { get; set; }

        public string AdminPanelInstance { get; set; } = string.Empty;
    }
}
