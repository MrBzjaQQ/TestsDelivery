using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TestsDelivery.Options.Tokens
{
    public class AuthOptions
    {
        private readonly string _key;

        public AuthOptions(string key)
        {
            _key = key;
        }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int Lifetime { get; set; }
        
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new(Encoding.ASCII.GetBytes(_key));
        }
    }
}