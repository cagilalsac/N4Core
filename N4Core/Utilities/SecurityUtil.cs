using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace N4Core.Utilities
{
    public class SecurityUtil
    {
        public SecurityKey GetSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public SigningCredentials GetSigningCredentials(string key, string algorithm = SecurityAlgorithms.HmacSha256Signature)
        {
            return new SigningCredentials(GetSecurityKey(key), algorithm);
        }
    }
}
