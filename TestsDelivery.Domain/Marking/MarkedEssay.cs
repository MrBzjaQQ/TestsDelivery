using TestsDelivery.Domain.Marking.Marks;

namespace TestsDelivery.Domain.Marking
{
    public record MarkedEssay : MarkedQuestionBase
    {
        public EssayMark Mark { get; set; }
    }
}
