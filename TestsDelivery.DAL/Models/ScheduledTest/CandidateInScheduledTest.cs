using System.ComponentModel.DataAnnotations.Schema;

namespace TestsDelivery.DAL.Models.ScheduledTest
{
    public record CandidateInScheduledTest : IdEntity<long>
    {
        [ForeignKey(nameof(Models.Candidate.Candidate))]
        public long CandidateId { get; set; }

        [ForeignKey(nameof(Models.ScheduledTest.ScheduledTest))]
        public long ScheduledTestId { get; set; }

        public virtual Candidate.Candidate Candidate { get; set; }

        public virtual ScheduledTest ScheduledTest { get; set; }
    }
}
