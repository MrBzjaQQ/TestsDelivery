using Microsoft.AspNetCore.Mvc;
using System;
using TestsDelivery.UserModels.TestProcess;
using TestsPortal.BL.Mediators.TestProcesses;

namespace TestsPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestProcessController : ControllerBase
    {
        private readonly ITestProcessMediator _testProcessMediator;

        public TestProcessController(ITestProcessMediator testProcessMediator)
        {
            _testProcessMediator = testProcessMediator;
        }

        [HttpGet]
        public IActionResult GetQuestionsForTest(long testId)
        {
            // TODO: check test is started
            return Ok(_testProcessMediator.GetQuestionsByTestId(testId));
        }

        [HttpGet]
        public IActionResult GetQuestionDetails(long questionId)
        {
            // TODO: check test is started
            return Ok(_testProcessMediator.GetQuestionById(questionId));
        }

        [HttpPost]
        public IActionResult PostAnswer()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IActionResult StartTest(StartTestModel model)
        {
            // TODO: candidate?
            // TODO: use PIN hash here
            // TODO: create token/guid for test
            // TODO: think about url format for test
            _testProcessMediator.StartTest(model);
            return Ok();
        }

        [HttpPost]
        public IActionResult FinishTest(long testId)
        {
            // TODO: create token/guid for test (?)
            _testProcessMediator.FinishTest(testId);
            return Ok();
        }
    }
}
