using Microsoft.AspNetCore.Mvc;
using N4Core.Managers.Bases;
using System.Globalization;

namespace N4Core.Controllers.Bases
{
    public abstract class MvcController : Controller
    {
        protected readonly CultureManagerBase _cultureManager;
        protected readonly CookieManagerBase _cookieManager;
        protected readonly SessionManagerBase _sessionManager;

        protected MvcController(CultureManagerBase cultureManager, CookieManagerBase cookieManager, SessionManagerBase sessionManager)
        {
            _cultureManager = cultureManager;
            _cookieManager = cookieManager;
            _sessionManager = sessionManager;
            string language = _cookieManager.GetCookie("lang");
            CultureInfo culture = _cultureManager.GetCulture(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
