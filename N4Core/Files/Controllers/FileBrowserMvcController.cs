#nullable disable

using Microsoft.AspNetCore.Mvc;
using N4Core.Culture;
using N4Core.Files.Enums;
using N4Core.Files.Utils.Bases;
using N4Core.Views.Models;

namespace N4Core.Files.Controllers
{
    public abstract class FileBrowserMvcController : Controller
    {
        protected readonly FileBrowserUtilBase _fileBrowserUtil;

        protected FileBrowserMvcController(FileBrowserUtilBase fileBrowserUtil)
        {
            _fileBrowserUtil = fileBrowserUtil;
        }

        public virtual IActionResult Index(string path = null)
        {
            var viewModel = _fileBrowserUtil.GetContents(path);
            if (viewModel is null)
                return View("Error", new ViewErrorModel(Languages.Türkçe));
            if (viewModel.FileType == FileTypes.Other)
                return File(viewModel.FileBinaryContent, viewModel.FileContentType, viewModel.Title);
            return View(viewModel);
        }
    }
}
