using System;
using System.ComponentModel.DataAnnotations.Schema;
using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.ScheduledTest
{
    public record ScheduledTest : IdEntity<long>
    {
        [ForeignKey(nameof(Models.Test.Test))]
        public long TestId { get; set; }

        public Test.Test Test { get; set; }

        public int Duration { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}
