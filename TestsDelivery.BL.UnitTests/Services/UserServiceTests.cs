using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TestsDelivery.BL.Models;
using TestsDelivery.BL.Services;
using TestsDelivery.BL.UnitTests.Services.Mocks;
using TestsDelivery.DAL.Models.Identity;
using TestsDelivery.Options.Tokens;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Services
{
    public class UserServiceTests
    {
        private const string AuthOptionsKey = "some key some key some key some key some key some key some key some key";
        private const string Email = "test@example.com";

        private readonly UserManagerMock<User> _userManagerMock;
        private readonly SignInManagerMock<User> _signInManagerMock;
        private readonly AuthOptions _authOptions;
        private readonly IdentityErrorDescriber _errorDescriber;
        private readonly UserService _service;

        public UserServiceTests()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            _errorDescriber = new IdentityErrorDescriber();

            serviceProviderMock.Setup(x => x.GetService(It.IsAny<Type>()))
                .Returns(new object());

            _userManagerMock = new UserManagerMock<User>(
                new Mock<IUserStore<User>>().Object,
               null,
                new Mock<IPasswordHasher<User>>().Object,
                null,
                null,
                new Mock<ILookupNormalizer>().Object,
                _errorDescriber,
                serviceProviderMock.Object,
                new Mock<ILogger<UserManager<User>>>().Object);

            _signInManagerMock = new SignInManagerMock<User>(
                _userManagerMock,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null,
                new Mock<ILogger<SignInManager<User>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<User>>().Object);

            _authOptions = new AuthOptions(AuthOptionsKey)
            {
                Audience = "you",
                Issuer = "me",
                Lifetime = 25200
            };

            _service = new UserService(
                _userManagerMock,
                _signInManagerMock,
                _errorDescriber,
                _authOptions);
        }

        [Fact]
        public async Task LoginUser_UserExists_Success()
        {
            // Arrange
            User user = new()
            {
                Id = "Id",
                UserName = "Some name",
                Email = Email
            };

            LoginRequestModel loginModel = new()
            {
                UserName = user.UserName,
                Password = "Some password"
            };

            _userManagerMock.SetupUser(user);

            _signInManagerMock.SetupSignInResult(SignInResult.Success);

            JwtSecurityToken expectedJwt = new(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_authOptions.Lifetime)),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512),
                claims: new Claim[]
                {
                    new("Id", user.Id),
                    new("UserName", user.UserName)
                });

            string expectedToken = new JwtSecurityTokenHandler().WriteToken(expectedJwt);

            // Act
            var loginResult = await _service.LoginUser(loginModel);

            // Assert
            Assert.True(loginResult.IsLoginSucceed);
            Assert.Equal(expectedToken, loginResult.LoginResponse.AccessToken);
            Assert.Equal(user.UserName, loginResult.LoginResponse.UserName);
        }

        [Fact]
        public async Task LoginUser_UserDoesNotExists_IncorrectPassword()
        {
            // Arrange
            LoginRequestModel loginModel = new()
            {
                UserName = "User Name",
                Password = "Some password"
            };

            // Act
            var loginResult = await _service.LoginUser(loginModel);

            // Assert
            Assert.False(loginResult.IsLoginSucceed);
            Assert.True(loginResult.Errors.Count > 0);
            Assert.Equal(_errorDescriber.PasswordMismatch().Code, loginResult.Errors[0].Code);
        }

        [Fact]
        public async Task LoginUser_UserExists_IncorrectPassword()
        {
            // Arrange
            User user = new()
            {
                Id = "Id",
                UserName = "Some name",
                Email = Email
            };

            LoginRequestModel loginModel = new()
            {
                UserName = user.UserName,
                Password = "Some password"
            };

            _userManagerMock.SetupUser(user);

            _signInManagerMock.SetupSignInResult(SignInResult.Failed);

            // Act
            var loginResult = await _service.LoginUser(loginModel);

            // Assert
            Assert.False(loginResult.IsLoginSucceed);
            Assert.True(loginResult.Errors.Count > 0);
            Assert.Equal(_errorDescriber.PasswordMismatch().Code, loginResult.Errors[0].Code);
        }

        [Fact]
        public async Task RegisterUser_UserDoesNotExists_Success()
        {
            // Arrange
            RegisterModelRequestModel model = new()
            {
                UserName = "UserName",
                Email = Email,
                Password = "Some Password"
            };

            _userManagerMock.SetupResult(IdentityResult.Success);

            // Act
            var registerResult = await _service.RegisterUser(model);

            // Assert
            Assert.True(registerResult.IsRegistrationSucceed);
            Assert.Equal(model.UserName, registerResult.RegisterResponse.UserName);
            Assert.Equal(model.Email, registerResult.RegisterResponse.Email);
        }

        [Fact]
        public async Task RegisterUser_UserExists_DuplicateEmailMessage()
        {
            // Arrange
            User user = new()
            {
                Id = "Id",
                UserName = "Some name",
                Email = Email
            };

            RegisterModelRequestModel model = new()
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = "Some Password"
            };

            _userManagerMock.SetupUser(user);
            _userManagerMock.SetupResult(IdentityResult.Failed());

            // Act
            var registerResult = await _service.RegisterUser(model);
            
            // Assert
            Assert.False(registerResult.IsRegistrationSucceed);
            Assert.True(registerResult.Errors.Count > 0);
            Assert.Equal(_errorDescriber.DuplicateEmail(user.Email).Code, registerResult.Errors[0].Code);
        }

        [Fact]
        public async Task RegisterUser_UserExists_DuplicateUserNameMessage()
        {
            // Arrange
            User user = new()
            {
                Id = "Id",
                UserName = "Some name"
            };

            RegisterModelRequestModel model = new()
            {
                UserName = user.UserName,
                Email = Email,
                Password = "Some Password"
            };

            _userManagerMock.SetupUser(user);
            _userManagerMock.SetupResult(IdentityResult.Failed());

            // Act
            var registerResult = await _service.RegisterUser(model);

            // Assert
            Assert.False(registerResult.IsRegistrationSucceed);
            Assert.True(registerResult.Errors.Count > 0);
            Assert.Equal(_errorDescriber.DuplicateUserName(user.UserName).Code, registerResult.Errors[0].Code);
        }
    }
}