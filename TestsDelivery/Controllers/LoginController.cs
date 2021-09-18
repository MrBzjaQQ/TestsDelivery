using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestsDelivery.DataTransferObjects;
using TestsDelivery.Logging;
using TestsDelivery.Services;

namespace TestsDelivery.Controllers
{
    /// <summary>
    /// Controller represents '/api/login' endpoints
    /// </summary>
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
        /// <summary>
        /// Method 'Login' handles POST request sent to '/api/login' endpoint
        /// </summary>
        /// <param name="model">Data provided from client.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
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

        [HttpGet]
        [Authorize]
        public IActionResult LoginByToken()
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                string userName = User.Claims.Single(claim => claim.Type == nameof(Models.Identity.User.UserName)).Value;

                return Ok(new LoginSucceedResponseDto
                {
                    AccessToken = token,
                    UserName = userName
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}