using Microsoft.AspNetCore.Mvc;
using TestsDelivery.Logging;

namespace TestsDelivery.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController: ControllerBase
    {
        private readonly IAppLogging<LoginController> _logger;

        public LoginController(IAppLogging<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogAppWarning("Warning Message");
            return Ok();
        }
    }
}