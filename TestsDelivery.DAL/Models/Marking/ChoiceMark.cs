using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.Marking
{
    public record ChoiceMark : MarkBase
    {
        public double Mark { get; set; }
    }
}
