﻿#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using N4Core.Controllers.Bases;
using N4Core.Managers.Bases;
using N4Core.Models;
using N4Core.Models.Accounts;
using N4Core.Services.Bases;
using N4Core.Utilities;

namespace N4Core.Controllers
{
    public class AccountController : MvcController
    {
        protected readonly AccountServiceBase _accountService;
        protected readonly AccountUtil _accountUtil;

        public AccountController(CultureManagerBase cultureManager, CookieManagerBase cookieManager, SessionManagerBase sessionManager, AccountServiceBase accountService) 
            : base(cultureManager, cookieManager, sessionManager)
        {
            _accountService = accountService;
            _accountService.Set(config => config.Language = cultureManager.GetLanguage());
            _accountUtil = new AccountUtil();
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
                    var principal = _accountUtil.GetPrincipal(result.Data);
                    await HttpContext.SignInAsync(_accountUtil.AuthenticationScheme, principal);
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
                await HttpContext.SignOutAsync(_accountUtil.AuthenticationScheme);
            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index", "Home");
            return Redirect(returnUrl);
        }

        public virtual IActionResult AccountAccessDenied()
        {
            return View("Error", new ErrorModel(_accountService.Messages.UserAccessDenied, _accountService.Language));
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
