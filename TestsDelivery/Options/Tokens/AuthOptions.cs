using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TestsDelivery.Options.Tokens
{
    public static class AuthOptions
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new(Encoding.ASCII.GetBytes(key));
        }
    }
}