using Microsoft.AspNetCore.Http;
using N4Core.Managers.Bases;

namespace N4Core.Managers
{
    public class SessionManager : SessionManagerBase
    {
        public SessionManager(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
