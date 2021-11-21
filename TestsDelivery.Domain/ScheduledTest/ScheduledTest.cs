using System;
using TestsDelivery.Domain.Test;

namespace TestsDelivery.Domain.ScheduledTest
{
    public record ScheduledTest
    {
        public long Id { get; set; }

        public Test.Test Test { get; set; }

        public Candidate.Candidate Candidate { get; set; }

        public TestStatus Status { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
