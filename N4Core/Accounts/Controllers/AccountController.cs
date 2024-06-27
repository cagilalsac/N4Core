#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using N4Core.Accounts.Models;
using N4Core.Accounts.Services.Bases;
using N4Core.Controllers.Bases;
using N4Core.Cookie.Utils.Bases;
using N4Core.Culture.Utils.Bases;
using N4Core.Session.Utils.Bases;
using N4Core.Views.Extensions;
using N4Core.Views.Models;

namespace N4Core.Accounts.Controllers
{
    public class AccountController : MvcController
    {
        protected readonly AccountServiceBase _accountService;

        public AccountController(CultureUtilBase cultureUtil, CookieUtilBase cookieUtil, SessionUtilBase sessionUtil, AccountServiceBase accountService)
            : base(cultureUtil, cookieUtil, sessionUtil)
        {
            _accountService = accountService;
            _accountService.Set(config => config.Language = cultureUtil.GetLanguage());
        }

        public virtual IActionResult AccountLogin(string returnUrl = null)
        {
            ViewBag.ViewModel = _accountService.ViewModel;
            var model = new AccountLoginModel()
            {
                ReturnUrl = Url.GetReturnRoute(returnUrl)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AccountLogin(AccountLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.GetPrincipal(model);
                if (response.IsSuccessful)
                {
                    await HttpContext.SignInAsync(_accountService.Config.AuthenticationScheme, response.Data);
                    return Redirect(Url.GetReturnRoute(model.ReturnUrl));
                }
                ModelState.AddModelError("", response.Message);
            }
            ViewBag.ViewModel = _accountService.ViewModel;
            return View(model);
        }

        public virtual async Task<IActionResult> AccountLogout(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync(_accountService.Config.AuthenticationScheme);
            return Redirect(Url.GetReturnRoute(returnUrl));
        }

        public virtual IActionResult AccountAccessDenied()
        {
            return View("Error", new ViewErrorModel(_accountService.Messages.UserAccessDenied, _accountService.Language));
        }

        public virtual IActionResult AccountRegister(string returnUrl = null)
        {
            var model = new AccountRegisterModel()
            {
                ReturnUrl = Url.GetReturnRoute(returnUrl)
            };
            ViewBag.ViewModel = _accountService.ViewModel;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AccountRegister(AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.RegisterUser(model);
                if (response.IsSuccessful)
                {
                    var loginModel = new AccountLoginModel()
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        ReturnUrl = Url.GetReturnRoute(model.ReturnUrl)
                    };
                    return await AccountLogin(loginModel);
                }
                ModelState.AddModelError("", response.Message);
            }
            ViewBag.ViewModel = _accountService.ViewModel;
            return View(model);
        }
    }
}
