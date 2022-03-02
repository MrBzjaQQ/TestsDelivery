using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Services.TestPortalInstances;

namespace AdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestPortalInstancesController : ControllerBase
    {
        private readonly ITestPortalInstancesService _service;

        public TestPortalInstancesController(ITestPortalInstancesService service)
        {
            _service = service;
        }

        [HttpGet("api/[controller]")]
        public IActionResult GetInstances()
        {
            return Ok(_service.GetInstances());
        }
    }
}
