using TestsDelivery.UserModels.Candidate;
using TestsDelivery.UserModels.Test;

namespace TestsDelivery.UserModels.ScheduledTest
{
    public record ScheduledTestDetailedModel
    {
        public long Id { get; set; }

        public IEnumerable<CandidateReadModel> Candidates { get; set; }

        public short TestStatus { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public TestDetailedModel Test { get; set;}
    }
}
