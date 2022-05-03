using Microsoft.AspNetCore.Mvc;
using System;
using TestsDelivery.BL.Mediators.TestProcesses;
using TestsDelivery.UserModels.AnsweredTests;

namespace AdminPanel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduledTestsController : ControllerBase
    {
        private readonly ITestProcessMediator _testProcessMediator;

        public ScheduledTestsController(ITestProcessMediator testProcessMediator)
        {
            _testProcessMediator = testProcessMediator;
        }

        [HttpPost]
        public IActionResult FinishTest(AnsweredTestCreateModel model)
        {
            _testProcessMediator.FinishTest(model);
            return Ok();
        }

        [HttpPost]
        public IActionResult StartTest(long testId)
        {
            _testProcessMediator.StartTest(testId);
            return Ok();
        }
    }
}
