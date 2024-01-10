using Microsoft.AspNetCore.Http;
using N4Core.Managers.Bases;

namespace N4Core.Managers
{
    public class CookieManager : CookieManagerBase
    {
        public CookieManager(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
