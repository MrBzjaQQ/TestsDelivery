using TestsDelivery.BL.Mediators.Marking;
using TestsDelivery.UserModels.Marking.Questions;

namespace AdminPanel.Controllers.Marking
{
    public class MultipleChoiceMarkController : MarkControllerBase<McqMarkCreateModel, McqMarkReadModel>
    {
        public MultipleChoiceMarkController(IMarkMediatorBase<McqMarkCreateModel, McqMarkReadModel> mediator)
            : base(mediator)
        {
        }
    }
}
