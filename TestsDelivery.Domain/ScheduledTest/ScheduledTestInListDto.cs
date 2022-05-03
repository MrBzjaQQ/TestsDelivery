using System;

namespace TestsDelivery.Domain.ScheduledTest
{
    public record ScheduledTestInListDto
    {
        public long Id { get; set; }

        public string TestName { get; set; }

        public string Pin { get; set; }

        public string Keycode { get; set; }

        public Candidate.Candidate Candidate { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
