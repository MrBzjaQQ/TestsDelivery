using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TestsDelivery.Models.Identity;

namespace TestsDelivery.UnitTests.Services.Mocks
{
    public class UserManagerMock<TUser>: UserManager<TUser>
        where TUser: User
    {
        private TUser _user;
        private IdentityResult _result;

        public UserManagerMock(
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public void SetupUser(TUser user)
        {
            _user = user;
        }

        public void SetupResult(IdentityResult result)
        {
            _result = result;
        }

        public override async Task<TUser> FindByNameAsync(string userName)
        {
            if (userName == _user?.UserName)
                return await Task.FromResult(_user);

            return await Task.FromResult<TUser>(null);
        }

        public override async Task<TUser> FindByEmailAsync(string email)
        {
            if (email == _user?.Email)
                return await Task.FromResult(_user);

            return await Task.FromResult<TUser>(null);
        }

        public override async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            _user = user;
            return await Task.FromResult(_result);
        }
    }
}
