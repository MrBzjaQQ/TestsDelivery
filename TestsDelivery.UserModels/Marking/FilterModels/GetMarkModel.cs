using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Marking.FilterModels
{
    public record GetMarkModel
    {
        [Range(1, long.MaxValue)]
        public long QuestionId { get; set; }

        [Range(1, long.MaxValue)]
        public long TestId { get; set; }
    }
}
