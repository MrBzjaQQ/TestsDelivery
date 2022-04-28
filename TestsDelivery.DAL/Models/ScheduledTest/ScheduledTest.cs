using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestsDelivery.DAL.Models.ScheduledTest
{
    public record ScheduledTest : IdEntity<long>
    {
        [ForeignKey(nameof(Models.Test.Test))]
        public long TestId { get; set; }

        public Test.Test Test { get; set; }

        public int Duration { get; set; }

        public string Keycode { get; set; }

        public string Pin { get; set; }

        public short Status { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
