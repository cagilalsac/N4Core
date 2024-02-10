#nullable disable

using Microsoft.AspNetCore.Mvc;
using N4Core.Managers.Bases;
using N4Core.Services.Bases;

namespace N4Core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        protected readonly AccountServiceBase _accountService;
        protected readonly JwtManagerBase _jwtManager;

        public AccountApiController(AccountServiceBase accountService, JwtManagerBase jwtManager)
        {
            _accountService = accountService;
            _jwtManager = jwtManager;
        }

        [HttpPost("[action]")]
        public virtual IActionResult TokenModel(string userName, string password)
        {
            if (ModelState.IsValid) 
            {
                var result = _accountService.GetUser(userName, password);
                if (result.IsSuccessful)
                    return Ok(_jwtManager.GetJwt(result.Data));
                ModelState.AddModelError("AccountApi", result.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("[action]")]
        public virtual string Token(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                var result = _accountService.GetUser(userName, password);
                if (result.IsSuccessful)
                    return _jwtManager.GetJwt(result.Data)?.Token;
                ModelState.AddModelError("AccountApi", result.Message);
            }
            return string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }
    }
}
