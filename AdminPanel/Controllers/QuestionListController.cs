﻿using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions.Lists;
using TestsDelivery.UserModels.Lists;

namespace AdminPanel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionListController : ControllerBase
    {
        private readonly IQuestionListsMediator _mediator;

        public QuestionListController(IQuestionListsMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult GetList(ListModel model)
        {
            return Ok(_mediator.GetList(model));
        }
    }
}
