using System;
using System.Threading.Tasks;
using TestsDelivery.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.UserModels.Register;
using TestsDelivery.BL.Services.Users;

namespace AdminPanel.Controllers
{
    /// <summary>
    /// Controller represents api/register endpoints
    /// </summary>
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

        /// <summary>
        /// Method Register handles POST request to the '/api/register'
        /// </summary>
        /// <param name="model">Information about information</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModelRequestModel model)
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
