using Microsoft.AspNetCore.Mvc;
using TestsPortal.BL.Mediators.Questions;

namespace TestsPortal.Controllers.Questions
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class QuestionControllerBase<TAnswerModel> : ControllerBase
    {
        protected IQuestionMediatorBase<TAnswerModel> Mediator { get; }

        public QuestionControllerBase(IQuestionMediatorBase<TAnswerModel> mediator)
        {
            Mediator = mediator;
        }
        
        [HttpPost]
        public IActionResult PostAnswer(TAnswerModel answer)
        {
            Mediator.PostAnswer(answer);
            return Ok();
        }
    }
}
