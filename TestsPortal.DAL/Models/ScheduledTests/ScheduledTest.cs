using System.ComponentModel.DataAnnotations.Schema;

namespace TestsPortal.DAL.Models.ScheduledTests
{
    public record ScheduledTest : IdOriginalIdEntity<long>
    {
        [ForeignKey(nameof(Tests.Test))]
        public long TestId { get; set; }

        public Tests.Test Test { get; set; }

        public int Duration { get; set; }

        public string AdminPanelInstance { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
