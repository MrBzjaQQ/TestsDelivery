using System;
using TestsDelivery.BL.Models.Candidate;

namespace TestsDelivery.BL.Models.ScheduledTest
{
    public record ScheduledTestReadModel
    {
        public long Id { get; set; }

        public CandidateReadModel Candidate { get; set; }

        public short TestStatus { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
