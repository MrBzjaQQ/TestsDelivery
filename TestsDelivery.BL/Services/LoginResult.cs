using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using TestsDelivery.UserModels.Login;

namespace TestsDelivery.Infrastructure.User
{
    public class LoginResult
    {
        public LoginResult()
        {
            Errors = new List<IdentityError>();
        }
        
        public bool IsLoginSucceed { get; set; }

        public LoginSucceedResponseModel LoginResponse { get; set; }
        
        public IList<IdentityError> Errors { get; set; }
    }
}