#nullable disable

using Microsoft.AspNetCore.Mvc;
using N4Core.Accounts.Models;
using N4Core.Accounts.Services.Bases;
using N4Core.Culture;
using N4Core.JsonWebToken.Utils.Bases;

namespace N4Core.Accounts.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        protected readonly AccountServiceBase _accountService;
        protected readonly JwtUtilBase _jwtUtil;

        public AccountApiController(AccountServiceBase accountService, JwtUtilBase jwtUtil)
        {
            _accountService = accountService;
            _accountService.Set(config => config.Language = Languages.English);
            _jwtUtil = jwtUtil;
        }

        [HttpPost("[action]")]
        public virtual async Task<IActionResult> TokenModel(AccountApiLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.GetUser(model.UserName, model.Password);
                if (response.IsSuccessful)
                    return Ok(_jwtUtil.GetJwt(response.Data));
                ModelState.AddModelError("AccountApi", response.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public virtual async Task<string> Token(AccountApiLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.GetUser(model.UserName, model.Password);
                if (response.IsSuccessful)
                    return _jwtUtil.GetJwt(response.Data)?.Token;
                ModelState.AddModelError("AccountApi", response.Message);
            }
            return string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }
    }
}
