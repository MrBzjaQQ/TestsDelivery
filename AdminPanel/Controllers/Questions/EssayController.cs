using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions;
using TestsDelivery.BL.Mediators.Questions.Essay;
using TestsDelivery.BL.Models.Questions.Essay;
using TestsDelivery.DAL.Exceptions.Questions;

namespace AdminPanel.Controllers.Questions
{
    // TODO: generalize controllers
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EssayController : ControllerBase
    {
        private readonly IEssayMediator _mediator;

        public EssayController(IEssayMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestion(int id)
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
        public IActionResult CreateQuestion(EssayCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var createdQuestion = _mediator.CreateQuestion(model);

                return CreatedAtRoute(nameof(GetQuestion), new { Id = createdQuestion.Id }, createdQuestion);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult EditQuestion(EssayEditModel model)
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
