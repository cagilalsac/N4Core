#nullable disable

using Microsoft.AspNetCore.Http;
using N4Core.Extensions;

namespace N4Core.Utilities
{
    public class SessionUtil
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public SessionUtil(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public void RemoveSession(string sessionKey)
		{
			_httpContextAccessor.HttpContext.Session.Remove(sessionKey);
		}

		public T GetSession<T>(string sessionKey) where T : class
		{
			return _httpContextAccessor.HttpContext.Session.GetObject<T>(sessionKey);
		}

		public void SetSession<T>(T sessionObject, string sessionKey) where T : class
		{
			_httpContextAccessor.HttpContext.Session.SetObject(sessionKey, sessionObject);
		}
	}
}
