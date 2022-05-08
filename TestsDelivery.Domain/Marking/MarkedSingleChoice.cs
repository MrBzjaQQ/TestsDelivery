namespace TestsDelivery.Domain.Marking
{
    public record MarkedSingleChoice : MarkedQuestionBase
    {
        public long Mark { get; set; }
    }
}
