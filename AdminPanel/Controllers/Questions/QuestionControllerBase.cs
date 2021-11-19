using System;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.Questions;
using TestsDelivery.BL.Models.Questions.BaseQuestion;
using TestsDelivery.DAL.Exceptions.Questions;

namespace AdminPanel.Controllers.Questions
{
    public abstract class QuestionControllerBase <TCreateModel, TEditModel, TReadModel> : ControllerBase
        where TCreateModel: BaseQuestionCreateModel
        where TEditModel: BaseQuestionEditModel
        where TReadModel: BaseQuestionReadModel
    {
        private readonly IBaseMediator<TCreateModel, TEditModel, TReadModel> _mediator;

        protected QuestionControllerBase(IBaseMediator<TCreateModel, TEditModel, TReadModel> mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}", Name = "GetQuestion")]
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
        public IActionResult CreateQuestion(TCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var createdQuestion = _mediator.CreateQuestion(model);

                return CreatedAtRoute(nameof(GetQuestion), new { Id = createdQuestion.Id }, createdQuestion);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult EditQuestion(TEditModel model)
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
