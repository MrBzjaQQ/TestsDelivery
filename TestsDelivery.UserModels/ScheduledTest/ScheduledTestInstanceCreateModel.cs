namespace TestsDelivery.UserModels.ScheduledTest
{
    public record ScheduledTestInstanceCreateModel
    {
        public long Id { get; set; }

        public Candidate.CandidateReadModel Candidate { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public short Status { get; set; }
    }
}
