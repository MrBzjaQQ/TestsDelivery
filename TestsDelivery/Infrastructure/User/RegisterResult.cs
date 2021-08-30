using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TestsDelivery.DataTransferObjects;

namespace TestsDelivery.Infrastructure.User
{
    public class RegisterResult
    {
        public RegisterResult()
        {
            Errors = new List<IdentityError>();
        }

        public bool IsRegistrationSucceed { get; set; }

        public RegisterSucceedResponseDto RegisterResponse { get; set; }

        public IList<IdentityError> Errors { get; set; }
    }
}
