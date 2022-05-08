using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions.MultipleChoice;
using TestsDelivery.UserModels.Marking.Questions;
using TestsDelivery.UserModels.Questions.MultipleChoice;

namespace AdminPanel.Controllers.Questions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MultipleChoiceQuestionController : QuestionControllerBase<McqCreateModel, McqEditModel, McqReadModel>
    {
        public MultipleChoiceQuestionController(IMcqMediator mediator)
            : base(mediator)
        {
        }
    }
}
