#nullable disable

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using N4Core.Settings.Bases;
using N4Core.Utilities;
using Newtonsoft.Json;

namespace N4Core.Settings
{
    public class JwtSettings : AppSettingsBase
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpirationInMinutes { get; set; }
        public string SecurityKey { get; set; }

        [JsonIgnore]
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

        [JsonIgnore]
        public SecurityKey SigningKey => new SecurityUtil().GetSecurityKey(SecurityKey);

        public JwtSettings(IConfiguration configuration, IWebHostEnvironment webHostEnvironment) : base(configuration, webHostEnvironment)
        {
            Name = nameof(JwtSettings);
        }

        public JwtSettings() : this(default, default)
        {
        }
    }
}
