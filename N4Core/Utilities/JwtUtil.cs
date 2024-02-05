#nullable disable

using Microsoft.IdentityModel.Tokens;
using N4Core.Extensions;
using N4Core.Models;
using N4Core.Models.Accounts;
using N4Core.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace N4Core.Utilities
{
    public class JwtUtil
    {
        private readonly SecurityUtil _securityUtil;

        public JwtUtil()
        {
            _securityUtil = new SecurityUtil();
        }

        public JwtUtil(AppSettingsUtil appSettingsUtil) : this()
        {
            appSettingsUtil.Bind<JwtSettings>();
        }

        public SecurityKey GetSigningKey(string securityKey) =>_securityUtil.GetSecurityKey(securityKey);

        public JwtModel GetJwt(AccountUserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.RoleName))
                return null;
            var signingCredentials = _securityUtil.GetSigningCredentials(JwtSettings.SecurityKey, JwtSettings.SecurityAlgorithm);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.Role, model.RoleName),
                new Claim(ClaimTypes.PrimarySid, model.Id.ToString())
            };
            var expiration = DateTime.Now.AddTime(0, JwtSettings.ExpirationInMinutes);
            var jwtSecurityToken = new JwtSecurityToken(JwtSettings.Issuer, JwtSettings.Audience, claims, DateTime.Now, expiration, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            return new JwtModel()
            {
                Token = "Bearer " + token,
                Expiration = expiration
            };
        }
    }
}
