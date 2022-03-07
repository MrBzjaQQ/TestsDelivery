using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions.SingleChoice;
using TestsDelivery.UserModels.Questions.SingleChoice;

namespace AdminPanel.Controllers.Questions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SingleChoiceQuestionController : QuestionControllerBase<ScqCreateModel, ScqEditModel, ScqReadModel>
    {
        public SingleChoiceQuestionController(IScqMediator mediator)
            : base(mediator)
        {
        }
    }
}
