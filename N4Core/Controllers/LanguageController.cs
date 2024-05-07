#nullable disable

using Microsoft.AspNetCore.Mvc;
using N4Core.Cookie.Utils.Bases;
using N4Core.Route.Utils;

namespace N4Core.Controllers
{
    public class LanguageController : Controller
    {
        private readonly CookieUtilBase _cookieUtil;

        public LanguageController(CookieUtilBase cookieUtil)
        {
            _cookieUtil = cookieUtil;
        }

        public virtual IActionResult Index(int language, string returnUrl = null)
        {
            _cookieUtil.Set("lang", language.ToString());
            return Redirect(MvcRouteUtil.GetReturnRoute(returnUrl));
        }
    }
}
