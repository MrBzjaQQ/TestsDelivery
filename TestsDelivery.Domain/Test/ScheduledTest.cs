namespace TestsDelivery.Domain.Test
{
    public record ScheduledTest : Test
    {
        public Candidate.Candidate Candidate { get; set; }

        public TestStatus Status { get; set; }

        public string Keycode { get; set; }

        public string Key { get; set; }

        public int Duration { get; set; }
    }
}
