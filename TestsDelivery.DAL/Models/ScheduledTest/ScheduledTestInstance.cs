using System.ComponentModel.DataAnnotations.Schema;
using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.ScheduledTest
{
    public record ScheduledTestInstance : IdEntity<long>
    {
        [ForeignKey(nameof(Models.Candidate.Candidate))]
        public long CandidateId { get; set; }

        [ForeignKey(nameof(Models.ScheduledTest.ScheduledTest))]
        public long ScheduledTestId { get; set; }

        public virtual Candidate.Candidate Candidate { get; set; }

        public virtual ScheduledTest ScheduledTest { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public short Status { get; set; }
    }
}
