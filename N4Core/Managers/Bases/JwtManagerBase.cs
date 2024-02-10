#nullable disable

using N4Core.Extensions;
using N4Core.Models;
using N4Core.Models.Accounts;
using N4Core.Settings;
using N4Core.Settings.Bases;
using N4Core.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace N4Core.Managers.Bases
{
    public abstract class JwtManagerBase
    {
        protected readonly SecurityUtil _securityUtil;

        public JwtSettings JwtSettings { get; protected set; }

        protected JwtManagerBase(AppSettingsBase appSettings)
        {
            JwtSettings = new JwtSettings();
            appSettings.Bind(JwtSettings);
            _securityUtil = new SecurityUtil();
        }

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
