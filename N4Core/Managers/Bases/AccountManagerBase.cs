#nullable disable

using Microsoft.AspNetCore.Http;
using N4Core.Entities;
using N4Core.Models;
using System.Security.Claims;

namespace N4Core.Managers.Bases
{
    public abstract class AccountManagerBase
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected AccountManagerBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual AccountUserModel GetUser()
        {
            AccountUserModel user = null;
            if (_httpContextAccessor is not null && _httpContextAccessor.HttpContext.User.Identity != null && _httpContextAccessor.HttpContext.User.Claims != null && _httpContextAccessor.HttpContext.User.Claims.Any())
            {
                var primarySidClaim = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(u => u.Type == ClaimTypes.PrimarySid);
                if (primarySidClaim != null)
                {
                    user = new AccountUserModel()
                    {
                        UserName = _httpContextAccessor.HttpContext.User.Identity.Name,
                        RoleNames = _httpContextAccessor.HttpContext.User.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList(),
                        Guid = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(u => u.Type == ClaimTypes.Sid)?.Value,
                        Id = Convert.ToInt32(primarySidClaim.Value)
                    };
                }
                else
                {
                    user = new AccountUserModel()
                    {
                        UserName = _httpContextAccessor.HttpContext.User.Identity.Name,
                        RoleNames = _httpContextAccessor.HttpContext.User.Claims.Where(u => u.Type == ClaimTypes.Role).Select(u => u.Value).ToList()
                    };
                }
            }
            return user;
        }

        public virtual AccountUserModel GetUser(AccountUser accountUser)
        {
            AccountUserModel user = null;
            if (accountUser != null && !string.IsNullOrWhiteSpace(accountUser.UserName) && accountUser.Role != null && !string.IsNullOrWhiteSpace(accountUser.Role.RoleName))
            {
                user = new AccountUserModel()
                {
                    UserName = accountUser.UserName,
                    Guid = accountUser.Guid,
                    Id = accountUser.Id,
                    RoleNames = new List<string>() { accountUser.Role.RoleName }
                };
            }
            return user;
        }
    }
}
