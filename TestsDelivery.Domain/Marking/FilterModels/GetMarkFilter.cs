namespace TestsDelivery.Domain.Marking.FilterModels
{
    public record GetMarkFilter
    {
        public long QuestionId { get; set; }

        public long TestId { get; set; }
    }
}
