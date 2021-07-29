using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using TestsDelivery.DataTransferObjects;

namespace TestsDelivery.Infrastructure.User
{
    public class LoginResult
    {
        public LoginResult()
        {
            Errors = new List<IdentityError>();
        }
        
        public bool IsLoginSucceed { get; set; }

        public LoginSucceedResponseDto LoginResponse { get; set; }
        
        public IList<IdentityError> Errors { get; set; }
    }
}