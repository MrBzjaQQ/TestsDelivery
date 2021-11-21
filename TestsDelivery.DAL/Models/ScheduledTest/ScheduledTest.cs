using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestsDelivery.DAL.Models.ScheduledTest
{
    public class ScheduledTest
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(Models.Test.Test))]
        public long TestId { get; set; }

        public Test.Test Test { get; set; }

        [ForeignKey(nameof(Models.Candidate.Candidate))]
        public long CandidateId { get; set; }

        public Candidate.Candidate Candidate { get; set; }

        public int Duration { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public short Status { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
