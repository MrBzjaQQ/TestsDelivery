using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using TestsDelivery.UserModels.Register;

namespace TestsDelivery.Infrastructure.User
{
    public class RegisterResult
    {
        public RegisterResult()
        {
            Errors = new List<IdentityError>();
        }

        public bool IsRegistrationSucceed { get; set; }

        public RegisterSucceedResponseModel RegisterResponse { get; set; }

        public IList<IdentityError> Errors { get; set; }
    }
}
