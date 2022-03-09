using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestsPortal.DAL.Models.ScheduledTests
{
    public record ScheduledTest
    {
        [Key]
        public long Id { get; set; }

        public long OriginalId { get; set; }

        [ForeignKey(nameof(Tests.Test))]
        public long TestId { get; set; }

        public Tests.Test Test { get; set; }

        [ForeignKey(nameof(Models.Candidate.Candidate))]
        public long CandidateId { get; set; }

        public Candidate.Candidate Candidate { get; set; }

        public int Duration { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
