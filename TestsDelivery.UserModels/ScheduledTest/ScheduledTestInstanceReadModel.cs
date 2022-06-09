using TestsDelivery.UserModels.Candidate;

namespace TestsDelivery.UserModels.ScheduledTest
{
    public record ScheduledTestInstanceReadModel
    {
        public long Id { get; set; }

        public CandidateReadModel Candidate { get; set; }

        public ScheduledTestReadModel ScheduledTest { get; set; }
    }
}
