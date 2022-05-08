using TestsDelivery.BL.Mediators.Marking;
using TestsDelivery.UserModels.Marking.Questions;

namespace AdminPanel.Controllers.Marking
{
    public class EssayMarkController : MarkControllerBase<EssayMarkCreateModel, EssayMarkReadModel>
    {
        public EssayMarkController(IMarkMediatorBase<EssayMarkCreateModel, EssayMarkReadModel> mediator)
            : base(mediator)
        {
        }
    }
}
