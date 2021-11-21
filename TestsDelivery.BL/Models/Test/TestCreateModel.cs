using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Test
{
    public class TestCreateModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<long> QuestionIds { get; set; }
    }
}
