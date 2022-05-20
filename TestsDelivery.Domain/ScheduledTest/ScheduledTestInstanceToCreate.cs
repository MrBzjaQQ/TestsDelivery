namespace TestsDelivery.Domain.ScheduledTest
{
    public record ScheduledTestInstanceToCreate
    {
        public long Id { get; set; }

        public Candidate.Candidate Candidate { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public TestStatus Status { get; set; }
    }
}
