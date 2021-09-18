﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TestsDelivery.DataTransferObjects;
using TestsDelivery.Infrastructure.User;
using TestsDelivery.Models.Identity;
using TestsDelivery.Options.Tokens;

namespace TestsDelivery.Services
{
    public class UserService : IUserService
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

        public async Task<LoginResult> LoginUser(LoginRequestDto model)
        {
            LoginResult result = new();

            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                result.Errors.Add(_describer.PasswordMismatch());
                return result;
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            result.IsLoginSucceed = signInResult.Succeeded;

            if (!signInResult.Succeeded)
            {
                result.Errors.Add(_describer.PasswordMismatch());
                return result;
            }

            var jwt = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_authOptions.Lifetime)),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512),
                claims: new Claim[]
                {
                    new(nameof(User.Id), user.Id),
                    new(nameof(User.UserName), user.UserName)
                });

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            result.LoginResponse = new LoginSucceedResponseDto
            {
                AccessToken = accessToken,
                UserName = user.UserName
            };

            return result;
        }

        public async Task<RegisterResult> RegisterUser(RegisterModelRequestDto model)
        {
            RegisterResult result = new();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                result.Errors.Add(_describer.DuplicateEmail(model.Email));
                return result;
            }

            user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null)
            {
                result.Errors.Add(_describer.DuplicateUserName(model.UserName));
                return result;
            }

            var identityResult = await _userManager.CreateAsync(
                new User
                {
                    UserName = model.UserName,
                    Email = model.Email
                },
                model.Password);

            result.IsRegistrationSucceed = identityResult.Succeeded;

            if (result.IsRegistrationSucceed)
            {
                user = await _userManager.FindByEmailAsync(model.Email);
                result.RegisterResponse = new RegisterSucceedResponseDto
                {
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName
                };
            }
            else
            {
                result.Errors = identityResult.Errors.ToList();
            }

            return result;
        }
    }
}