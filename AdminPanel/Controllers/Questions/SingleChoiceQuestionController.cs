using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions;
using TestsDelivery.BL.Mediators.Questions.SingleChoice;
using TestsDelivery.BL.Models.Questions.SingleChoice;
using TestsDelivery.DAL.Exceptions.Questions;

namespace AdminPanel.Controllers.Questions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SingleChoiceQuestionController : ControllerBase
    {
        private readonly IScqMediator _mediator;

        public SingleChoiceQuestionController(IScqMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}", Name = "GetQuestion")]
        // TODO: Get does not return options
        public IActionResult GetQuestion(long id)
        {
            if (id < 1)
                return BadRequest(new ArgumentException(nameof(id)));

            try
            {
                var question = _mediator.GetQuestion(id);

                return Ok(question);
            }
            catch (QuestionException ex)
            {
                ModelState.AddModelError("QuestionException", ex.Message);

                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IActionResult CreateQuestion(ScqCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var createdQuestion = _mediator.CreateQuestion(model);

                return CreatedAtRoute(nameof(GetQuestion), new { Id = createdQuestion.Id }, createdQuestion);
            }
            
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult EditQuestion(ScqEditModel model)
        {
            if (ModelState.IsValid)
            {
                _mediator.EditQuestion(model);

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
