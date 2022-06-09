using TestsDelivery.BL.Mediators.Marking;
using TestsDelivery.UserModels.Marking.Questions;

namespace AdminPanel.Controllers.Marking
{
    public class SingleChoiceMarkController : MarkControllerBase<ScqMarkCreateModel, ScqMarkReadModel>
    {
        public SingleChoiceMarkController(IMarkMediatorBase<ScqMarkCreateModel, ScqMarkReadModel> mediator)
            : base(mediator)
        {
        }
    }
}
