using System;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Exceptions.Validation;
using TestsDelivery.BL.Mediators.Questions;
using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.DAL.Exceptions.Questions;

namespace AdminPanel.Controllers.Questions
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class QuestionControllerBase <TCreateModel, TEditModel, TReadModel> : ControllerBase
        where TCreateModel: QuestionCreateModel
        where TEditModel: QuestionEditModel
        where TReadModel: QuestionReadModel
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

        [HttpDelete("{id}")]
        [ActionName(nameof(DeleteQuestion))]
        public IActionResult DeleteQuestion(long id)
        {
            if (id < 1)
                return BadRequest(new ArgumentException(nameof(id)));

            try
            {
                _mediator.DeleteQuestion(id);

                return Ok();
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
