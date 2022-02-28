using Microsoft.AspNetCore.Mvc;
using System;

namespace TestsPortal.Controllers
{
    [ApiController]
    public class ScheduledTestsController : ControllerBase
    {
        public ScheduledTestsController()
        {

        }

        [HttpPost]
        public IActionResult ScheduleTest()
        {
            throw new NotImplementedException();
        }
    }
}
