using N4Core.Models.Accounts;
using System.Security.Claims;

namespace N4Core.Utilities
{
    public class AccountUtil
    {
        public string AuthenticationScheme { get; set; } = "AccountAuthScheme";

        public ClaimsPrincipal GetPrincipal(AccountUserModel model)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.Role, model.RoleName),
                new Claim(ClaimTypes.PrimarySid, model.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }
    }
}
