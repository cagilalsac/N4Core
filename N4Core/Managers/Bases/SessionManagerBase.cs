#nullable disable

using Microsoft.AspNetCore.Http;
using N4Core.Extensions;

namespace N4Core.Managers.Bases
{
    public abstract class SessionManagerBase
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected SessionManagerBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual void RemoveSession(string sessionKey)
        {
            _httpContextAccessor.HttpContext.Session.Remove(sessionKey);
        }

        public virtual T GetSession<T>(string sessionKey) where T : class
        {
            return _httpContextAccessor.HttpContext.Session.GetObject<T>(sessionKey);
        }

        public virtual void SetSession<T>(T sessionObject, string sessionKey) where T : class
        {
            _httpContextAccessor.HttpContext.Session.SetObject(sessionKey, sessionObject);
        }
    }
}
