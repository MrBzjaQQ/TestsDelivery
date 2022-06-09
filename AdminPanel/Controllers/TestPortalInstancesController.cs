using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.BL.Services.TestPortalInstances;

namespace AdminPanel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestPortalInstancesController : ControllerBase
    {
        private readonly ITestPortalInstancesService _service;

        public TestPortalInstancesController(ITestPortalInstancesService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetInstances()
        {
            return Ok(_service.GetInstances());
        }
    }
}
