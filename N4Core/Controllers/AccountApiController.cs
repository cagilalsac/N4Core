#nullable disable

using Microsoft.AspNetCore.Mvc;
using N4Core.Services.Bases;
using N4Core.Utilities;

namespace N4Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        protected readonly AccountServiceBase _accountService;
        protected readonly JwtUtil _jwtUtil;

        public AccountApiController(AccountServiceBase accountService)
        {
            _accountService = accountService;
            _jwtUtil = new JwtUtil();
        }

        [HttpPost("TokenModel")]
        public virtual IActionResult TokenModel(string userName, string password)
        {
            if (ModelState.IsValid) 
            {
                var result = _accountService.GetUser(userName, password);
                if (result.IsSuccessful)
                    return Ok(_jwtUtil.GetJwt(result.Data));
                ModelState.AddModelError("AccountApi", result.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Token")]
        public virtual string Token(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.GetUser(userName, password);
                if (result.IsSuccessful)
                    return _jwtUtil.GetJwt(result.Data)?.Token;
                ModelState.AddModelError("AccountApi", result.Message);
            }
            return string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }
    }
}
