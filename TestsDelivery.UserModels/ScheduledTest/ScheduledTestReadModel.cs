using System;
using System.Collections.Generic;
using TestsDelivery.UserModels.Candidate;

namespace TestsDelivery.UserModels.ScheduledTest
{
    public record ScheduledTestReadModel
    {
        public long Id { get; set; }

        public IEnumerable<CandidateReadModel> Candidates { get; set; }

        public short TestStatus { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
