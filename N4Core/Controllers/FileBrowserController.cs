#nullable disable

using Microsoft.AspNetCore.Mvc;
using N4Core.Controllers.Bases;
using N4Core.Managers.Bases;
using N4Core.Services.Bases;

namespace N4Core.Controllers
{
    public class FileBrowserController : MvcControllerBase
    {
        protected readonly FileBrowserServiceBase _fileBrowserService;

        public FileBrowserController(CultureManagerBase cultureManager, CookieManagerBase cookieManager, FileBrowserServiceBase fileBrowserService) : base(cultureManager, cookieManager)
        {
            _fileBrowserService = fileBrowserService;
        }

        public virtual IActionResult Index(string path = null)
        {
            var viewModel = _fileBrowserService.GetContents(path);
            if (viewModel is null)
                return BadRequest();
            return Json(viewModel);
        }
    }
}
