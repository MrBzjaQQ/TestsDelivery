using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TestsDelivery.Infrastructure.Logging;
using TestsDelivery.UserModels.ScheduledTest;
using TestsPortal.BL.Mediators.ScheduledTest;

namespace TestsPortal.Controllers
{
    [ApiController]
    public class ScheduledTestsController : ControllerBase
    {
        private readonly IScheduledTestMediator _mediator;
        private readonly IAppLogging<ScheduledTestsController> _logger;

        public ScheduledTestsController(
            IScheduledTestMediator mediator,
            IAppLogging<ScheduledTestsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult ScheduleTest(ScheduledTestDetailedModel detailedModel)
        {
            _logger.LogAppInformation($"Received data from {Request.Host}: {JsonSerializer.Serialize(detailedModel)}");
            return Ok();
        }
    }
}
