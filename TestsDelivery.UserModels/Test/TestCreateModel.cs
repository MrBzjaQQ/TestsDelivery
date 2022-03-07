using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Test
{
    public class TestCreateModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public IEnumerable<long> QuestionIds { get; set; }
    }
}
