using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions;
using TestsDelivery.BL.Mediators.Questions.MultipleChoice;
using TestsDelivery.BL.Models.Questions.MultipleChoice;
using TestsDelivery.DAL.Exceptions.Questions;

namespace AdminPanel.Controllers.Questions
{
    // TODO: Probably it is possible to generalize controllers
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MultipleChoiceQuestionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMcqMediator _mediator;

        public MultipleChoiceQuestionController(
            IMcqMediator mediator,
            IMapper mapper)
        {
            _mapper = mapper;
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
        public IActionResult CreateQuestion(McqCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var createdQuestion = _mediator.CreateQuestion(model);

                return CreatedAtRoute(nameof(GetQuestion), new { Id = createdQuestion.Id }, createdQuestion);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult EditQuestion(McqEditModel model)
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
