using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TestsDelivery.BL.UnitTests.Services.Mocks
{
    public class SignInManagerMock<TUser>: SignInManager<TUser>
        where TUser: class
    {
        private SignInResult _signInResult;

        public SignInManagerMock(
            UserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<TUser> confirmation) 
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }

        public void SetupSignInResult(SignInResult result)
        {
            _signInResult = result;
        }

        public override async Task<SignInResult> CheckPasswordSignInAsync(
            TUser user,
            string password,
            bool lockoutOnFailure)
        {
            return await Task.FromResult(_signInResult);
        }
    }
}
