using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Mediators.ScheduledTest;
using TestsDelivery.UserModels.ScheduledTest;
using TestsDelivery.DAL.Exceptions.Candidate;
using TestsDelivery.DAL.Exceptions.ScheduledTest;
using TestsDelivery.BL.Shared.Clients.Integration;
using System.Threading.Tasks;

namespace AdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduledTestMediator _mediator;

        public ScheduleController(IScheduledTestMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleTest(ScheduledTestCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var test = await _mediator.ScheduleTest(model);
                    return CreatedAtRoute(nameof(GetScheduledTest), new { test.Id }, test);
                }
                catch(IntegrationException ex)
                {
                    ModelState.AddModelError("IntegrationException", ex.Message);
                    return Problem(detail: ex.Message);
                }
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
