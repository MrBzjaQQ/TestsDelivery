using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions.Lists;
using TestsDelivery.UserModels.ListFilters;

namespace AdminPanel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestionListController : ControllerBase
    {
        private readonly IQuestionListsMediator _mediator;

        public QuestionListController(IQuestionListsMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(nameof(GetQuestionsForSubject))]
        public IActionResult GetQuestionsForSubject(QuestionsInSubjectListFilterModel model)
        {
            return Ok(_mediator.GetQuestionsForSubject(model));
        }

        [HttpPost(nameof(GetQuestionsInSubjects))]
        public IActionResult GetQuestionsInSubjects(ListFilterModel model)
        {
            return Ok(_mediator.GetQuestionsInSubjects(model));
        }
    }
}
