namespace TestsDelivery.Domain.Marking
{
    public record MarkedMultipleChoice : MarkedQuestionBase
    {
        public long Mark { get; set; }
    }
}
