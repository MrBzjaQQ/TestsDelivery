using System;
using System.Collections.Generic;
using TestsDelivery.BL.Models.Candidate;
using TestsDelivery.BL.Models.Test;

namespace TestsDelivery.BL.Models.ScheduledTest
{
    public record ScheduledTestDetailedModel
    {
        public long Id { get; set; }

        public IEnumerable<CandidateReadModel> Candidates { get; set; }

        public short TestStatus { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public TestDetailedModel Test {get;set;}
    }
}
