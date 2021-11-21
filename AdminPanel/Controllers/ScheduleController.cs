using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.ScheduledTest;
using TestsDelivery.BL.Models.ScheduledTest;
using TestsDelivery.DAL.Exceptions.Candidate;
using TestsDelivery.DAL.Exceptions.ScheduledTest;

namespace AdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduledTestMediator _mediator;

        public ScheduleController(IScheduledTestMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult ScheduleTest(ScheduledTestCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var test = _mediator.ScheduleTest(model);

                return CreatedAtRoute(nameof(GetScheduledTest), new {test.Id}, test);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("Test/{id}", Name = "GetScheduledTest")]
        public IActionResult GetScheduledTest(long id)
        {
            if (id < 1)
            {
                ModelState.TryAddModelError("IdOutOfRange", $"Id = {id} is out of range");
                return BadRequest(ModelState);
            }

            try
            {
                var test = _mediator.GetTest(id);
                return Ok(test);
            }
            catch (ScheduledTestException ex)
            {
                ModelState.TryAddModelError(nameof(ScheduledTestException), ex.Message);
                return BadRequest(ModelState);
            }
            catch (CandidateException ex)
            {
                ModelState.TryAddModelError(nameof(CandidateException), ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
