namespace TestsPortal.Domain.ScheduledTests
{
    public record ScheduledTestToCreate
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public Tests.Test Test { get; set; }

        public string AdminPanelInstance { get; set; }

        public IEnumerable<ScheduledTestInstanceToCreate> Instances { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
