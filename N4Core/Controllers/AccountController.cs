#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using N4Core.Controllers.Bases;
using N4Core.Managers.Bases;
using N4Core.Models;
using N4Core.Services.Bases;
using System.Security.Claims;

namespace N4Core.Controllers
{
    public class AccountController : MvcController
    {
        protected readonly AccountServiceBase _accountService;

        public AccountController(CultureManagerBase cultureManager, CookieManagerBase cookieManager, SessionManagerBase sessionManager, AccountServiceBase accountService) : base(cultureManager, cookieManager, sessionManager)
        {
            _accountService = accountService;
            _accountService.Set(config =>
            {
                config.Language = cultureManager.GetLanguage();
                config.AuthenticationScheme = "Cookies";
            });
        }

        public virtual IActionResult AccountLogin(string returnUrl = null)
        {
            ViewBag.ViewModel = _accountService.ViewModel;
            var model = new AccountLoginModel()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> AccountLogin(AccountLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.GetUser(model);
                if (result.IsSuccessful)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, result.Data.UserName),
                        new Claim(ClaimTypes.Role, result.Data.Role),
                        new Claim(ClaimTypes.PrimarySid, result.Data.Id.ToString())
                    };
                    var identity = new ClaimsIdentity(claims, _accountService.Config.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(_accountService.Config.AuthenticationScheme, principal);
                    if (string.IsNullOrWhiteSpace(model.ReturnUrl))
                        return RedirectToAction("Index", "Home");
                    return Redirect(model.ReturnUrl);
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.ViewModel = _accountService.ViewModel;
            return View(model);
        }

        public virtual async Task<IActionResult> AccountLogout(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync(_accountService.Config.AuthenticationScheme);
            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index", "Home");
            return Redirect(returnUrl);
        }

        public virtual IActionResult AccountAccessDenied()
        {
            return View("Error", new ErrorModel(_accountService.Messages.UserAccessDenied, _accountService.Config.Language));
        }

        public virtual IActionResult AccountRegister(string returnUrl = null)
        {
            var model = new AccountRegisterModel()
            {
                ReturnUrl = returnUrl
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
                var result = _accountService.RegisterUser(model);
                if (result.IsSuccessful)
                {
                    var loginModel = new AccountLoginModel()
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        ReturnUrl = model.ReturnUrl
                    };
                    return await AccountLogin(loginModel);
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.ViewModel = _accountService.ViewModel;
            return View(model);
        }
    }
}
