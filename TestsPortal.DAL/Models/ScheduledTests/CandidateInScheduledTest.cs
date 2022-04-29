using System.ComponentModel.DataAnnotations.Schema;
using TestsPortal.DAL.Models.Candidates;

namespace TestsPortal.DAL.Models.ScheduledTests
{
    public record CandidateInScheduledTest : IdEntity<long>
    {
        [ForeignKey(nameof(Candidate))]
        public long CandidateId { get; set; }

        [ForeignKey(nameof(ScheduledTests.ScheduledTest))]
        public long ScheduledTestId { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual ScheduledTest ScheduledTest { get; set; }
    }
}
