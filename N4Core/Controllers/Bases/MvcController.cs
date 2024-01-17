using Microsoft.AspNetCore.Mvc;
using N4Core.Enums;
using N4Core.Managers.Bases;
using System.Globalization;

namespace N4Core.Controllers.Bases
{
    public abstract class MvcController : Controller
    {
        protected readonly CultureManagerBase _cultureManager;
        protected readonly CookieManagerBase _cookieManager;

        protected MvcController(CultureManagerBase cultureManager, CookieManagerBase cookieManager)
        {
            _cultureManager = cultureManager;
            _cookieManager = cookieManager;
            string language = _cookieManager.GetCookie(nameof(Languages));
            CultureInfo culture = _cultureManager.GetCulture(language);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
