#nullable disable

using Microsoft.AspNetCore.Mvc;
using N4Core.Enums;
using N4Core.Models;
using N4Core.Services.Bases;

namespace N4Core.Controllers
{
    public abstract class FileBrowserController : Controller
    {
        protected readonly FileBrowserServiceBase _fileBrowserService;

        protected FileBrowserController(FileBrowserServiceBase fileBrowserService)
        {
            _fileBrowserService = fileBrowserService;
        }

        public virtual IActionResult Index(string path = null)
        {
            var viewModel = _fileBrowserService.GetContents(path);
            if (viewModel is null)
                return View("Error", new ErrorModel(Languages.Türkçe));
            if (viewModel.FileType == FileTypes.Other)
                return File(viewModel.FileBinaryContent, viewModel.FileContentType, viewModel.Title);
            return View(viewModel);
        }
    }
}
