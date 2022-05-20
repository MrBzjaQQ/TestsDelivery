namespace TestsDelivery.Domain.ScheduledTest
{
    public record ScheduledTestInstance
    {
        public long Id { get; set; }

        public Candidate.Candidate Candidate { get; set; }

        public ScheduledTest ScheduledTest { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public short Status { get; set; }
    }
}
