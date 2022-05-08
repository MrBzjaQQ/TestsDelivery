using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

        [HttpGet(nameof(QuestionsForTest))]
        public IActionResult QuestionsForTest(long testId)
        {
            // TODO: check test is started
            return Ok(_testProcessMediator.GetQuestionsByTestId(testId));
        }

        [HttpGet(nameof(QuestionDetails))]
        public IActionResult QuestionDetails(long questionId)
        {
            // TODO: check test is started
            return Ok(_testProcessMediator.GetQuestionById(questionId));
        }

        [HttpPost(nameof(StartTest))]
        public IActionResult StartTest(StartTestModel model)
        {
            // TODO: use PIN hash here
            // TODO: create token/guid for test
            // TODO: think about url format for test
            _testProcessMediator.StartTest(model);
            return Ok();
        }

        [HttpPost(nameof(FinishTest))]
        public async Task<IActionResult> FinishTest(long testId)
        {
            // TODO: create token/guid for test (?)
            await _testProcessMediator.FinishTest(testId);
            return Ok();
        }
    }
}
