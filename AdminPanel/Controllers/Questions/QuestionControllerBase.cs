using System;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Exceptions.Validation;
using TestsDelivery.BL.Mediators.Questions;
using TestsDelivery.BL.Models.Questions.BaseQuestion;
using TestsDelivery.DAL.Exceptions.Questions;

namespace AdminPanel.Controllers.Questions
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet("{id}")]
        [ActionName(nameof(GetQuestion))]
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

                return CreatedAtAction(nameof(GetQuestion), createdQuestion);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult EditQuestion(TEditModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _mediator.EditQuestion(model);
                }
                catch (QuestionValidationException ex)
                {
                    ModelState.AddModelError("QuestionException", ex.Message);

                    return BadRequest(ModelState);
                }

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}
