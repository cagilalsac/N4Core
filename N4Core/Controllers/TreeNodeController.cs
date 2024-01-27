using Microsoft.AspNetCore.Mvc;
using N4Core.Configurations;
using N4Core.Services.Bases;

namespace N4Core.Controllers
{
    public abstract class TreeNodeController : Controller
    {
        protected readonly TreeNodeServiceBase _treeNodeService;

        protected TreeNodeController(TreeNodeServiceBase treeNodeService)
        {
            _treeNodeService = treeNodeService;
        }

        public virtual IActionResult Index(TreeNodeServiceConfig config)
        {
            _treeNodeService.Set(c =>
            {
                c.Language = config.Language;
                c.ShowDetailTexts = config.ShowDetailTexts;
                c.ShowAbbreviations = config.ShowAbbreviations;
                c.ShowOnlyActive = config.ShowOnlyActive;
            });
            return Ok(_treeNodeService.GetJqueryOrgchartNodes());
        }
    }
}
