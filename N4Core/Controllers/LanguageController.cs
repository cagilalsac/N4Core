﻿#nullable disable

using Microsoft.AspNetCore.Mvc;
using N4Core.Enums;
using N4Core.Managers.Bases;
using N4Core.Utilities;

namespace N4Core.Controllers
{
    public class LanguageController : Controller
    {
        protected readonly CookieManagerBase _cookieManager;

        public LanguageController(CookieManagerBase cookieManager)
        {
            _cookieManager = cookieManager;
        }

        public virtual IActionResult Index(int language, string returnUrl = null)
        {
            _cookieManager.SetCookie("lang", language.ToString());
            if (returnUrl is null)
                return Redirect(MvcRouteUtil.GetHomeRoute());
            return Redirect(returnUrl);
        }
    }
}
