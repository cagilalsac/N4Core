#nullable disable

using Microsoft.AspNetCore.Http;
using N4Core.Models;

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
                Expires = new ExpireModel().DateTimeOffset,
                HttpOnly = true
            };
            SetCookie(key, value, cookieOptions);
        }

        public virtual void SetCookie(string key, string value, ExpireModel expireModel)
        {
            var cookieOptions = new CookieOptions()
            {
                Expires = expireModel.DateTimeOffset,
                HttpOnly = true
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
                Expires = new ExpireModel(-1).DateTimeOffset,
                HttpOnly = true
            };
            SetCookie(key, string.Empty, cookieOptions);
        }
    }
}
