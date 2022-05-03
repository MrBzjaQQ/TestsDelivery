using TestsDelivery.UserModels.Candidate;

namespace TestsDelivery.UserModels.ScheduledTest
{
    public class ScheduledTestInListModel
    {
        public long Id { get; set; }

        public string TestName { get; set; }

        public string Pin { get; set; }

        public string Keycode { get; set; }

        public CandidateReadModel Candidate { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
