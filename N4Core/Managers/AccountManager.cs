using Microsoft.AspNetCore.Http;
using N4Core.Managers.Bases;

namespace N4Core.Managers
{
    public class AccountManager : AccountManagerBase
    {
        public AccountManager(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
