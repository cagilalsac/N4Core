#nullable disable

using Microsoft.IdentityModel.Tokens;

namespace N4Core.Settings
{
    public class JwtSettings
    {
        public static string Audience { get; set; }
        public static string Issuer { get; set; }
        public static int ExpirationInMinutes { get; set; }
        public static string SecurityKey { get; set; }
        public static string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
    }
}
