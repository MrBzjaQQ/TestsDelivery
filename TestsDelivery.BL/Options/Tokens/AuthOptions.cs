using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TestsDelivery.Options.Tokens
{
    public class AuthOptions
    {
        private readonly string _issuerSigningKey;
        private readonly string _tokenValidationKey;

        public AuthOptions(string issuerSigningKey)
        {
            _issuerSigningKey = issuerSigningKey;
        }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int Lifetime { get; set; }
        
        public SymmetricSecurityKey GetIssuerSigningKey()
        {
            return new(Encoding.ASCII.GetBytes(_issuerSigningKey));
        }
    }
}