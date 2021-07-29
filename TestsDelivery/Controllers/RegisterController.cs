using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestsDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger _logger;

        public RegisterController(ILogger logger)
        {
            _logger = logger;
        }
    }
}
