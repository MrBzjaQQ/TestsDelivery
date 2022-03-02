using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.ScheduledTest
{
    public record ScheduledTestCreateModel
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long TestId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpirationDateTime { get; set; }

        [Required]
        [Range(1, long.MaxValue)]
        public int Duration { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<long> CandidateIds { get; set; }

        public string DestinationInstance { get; set; }
    }
}
