using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TestsDelivery.DataTransferObjects;
using TestsDelivery.Infrastructure.User;
using TestsDelivery.Models.Identity;
using TestsDelivery.Options.Tokens;

namespace TestsDelivery.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IdentityErrorDescriber _describer;
        private readonly AuthOptions _authOptions;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IdentityErrorDescriber describer,
            AuthOptions authOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _describer = describer;
            _authOptions = authOptions;
        }

        public async Task<LoginResult> LoginUser(LoginModel model)
        {
            LoginResult result = new();
            
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                result.Errors.Add(_describer.InvalidUserName(model.UserName));
                return result;
            }

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_authOptions.Lifetime)),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            result.LoginResponse = new LoginSucceedResponseDto
            {
                AccessToken = accessToken,
                Email = user.Email
            };

            return result;
        }
    }
}