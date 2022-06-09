using TestsDelivery.UserModels.Marking.MarkModels;

namespace TestsDelivery.UserModels.Marking.Questions
{
    public record EssayMarkCreateModel : MarkCreateModelBase
    {
        public EssayMarkModel Mark { get;set; } 
    }
}
