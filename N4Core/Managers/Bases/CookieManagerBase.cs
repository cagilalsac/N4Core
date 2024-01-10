using Microsoft.AspNetCore.Http;

namespace N4Core.Managers.Bases
{
    public abstract class CookieManagerBase
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected CookieManagerBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual void SetCookie(string key, string value, CookieOptions cookieOptions)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, cookieOptions);
        }

        public virtual void SetCookie(string key, string value)
        {
            var cookieOptions = new CookieOptions()
            {
                Expires = DateTimeOffset.MaxValue
            };
            SetCookie(key, value, cookieOptions);
        }

        public virtual string GetCookie(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        public virtual void RemoveCookie(string key)
        {
            var cookieOptions = new CookieOptions()
            {
                Expires = DateTimeOffset.Now.AddDays(-1)
            };
            SetCookie(key, string.Empty, cookieOptions);
        }
    }
}
