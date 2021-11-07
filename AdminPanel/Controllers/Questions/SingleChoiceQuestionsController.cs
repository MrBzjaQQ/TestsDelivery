using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions;
using TestsDelivery.BL.Models.Questions.SingleChoice;
using TestsDelivery.DAL.Exceptions.Questions;

namespace AdminPanel.Controllers.Questions
{
    [Route("api/[controller]")]
    [ApiController]
    public class SingleChoiceQuestionsController : ControllerBase
    {
        private readonly IScqMediator _mediator;

        public SingleChoiceQuestionsController(IScqMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}", Name = "GetQuestion")]
        [Authorize]
        public IActionResult GetQuestion(long id)
        {
            if (id < 1)
                return BadRequest(new ArgumentException(nameof(id)));

            try
            {
                var question = _mediator.GetQuestion(id);

                return Ok(question);
            }
            catch(QuestionException ex)
            {
                ModelState.AddModelError("QuestionException", ex.Message);

                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize]
        // TODO: Text is not created
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
        [Authorize]
        // TODO: question was not found by id
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
