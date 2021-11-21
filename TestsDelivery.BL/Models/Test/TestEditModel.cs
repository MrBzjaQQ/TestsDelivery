using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Test
{
    public record TestEditModel
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        public IEnumerable<long> QuestionIds { get; set; }
    }
}
