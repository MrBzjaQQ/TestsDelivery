using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.DataTransferObjects;
using TestsDelivery.Logging;
using TestsDelivery.Models.Identity;
using TestsDelivery.Services;

namespace TestsDelivery.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController: ControllerBase
    {
        private readonly IAppLogging<LoginController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public LoginController(
            IAppLogging<LoginController> logger,
            IUserService service,
            IMapper mapper)
        {
            _logger = logger;
            _userService = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUser(model);

                if (result.IsLoginSucceed)
                {
                    return Ok(result.LoginResponse);
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