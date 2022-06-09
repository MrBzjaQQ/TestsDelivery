using System;
using System.Collections.Generic;

namespace TestsDelivery.Domain.ScheduledTest
{
    public record ScheduledTest
    {
        public long Id { get; set; }

        public Test.Test Test { get; set; }

        public IEnumerable<Candidate.Candidate> Candidates { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
