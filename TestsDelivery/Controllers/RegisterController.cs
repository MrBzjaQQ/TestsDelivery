using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.Logging;
using TestsDelivery.Models.Identity;
using TestsDelivery.Services;

namespace TestsDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IAppLogging<RegisterController> _logger;
        private readonly IUserService _service;

        public RegisterController(
            IAppLogging<RegisterController> logger,
            IUserService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.RegisterUser(model);

                if (result.IsRegistrationSucceed)
                {
                    return Created(new Uri("api/Register", UriKind.Relative), result.RegisterResponse);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }
    }
}
