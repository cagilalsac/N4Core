using N4Core.Culture.Utils.Bases;
using N4Core.Files.Entities;
using N4Core.Files.Models;
using N4Core.Files.Services.Bases;
using N4Core.Mappers.Utils.Bases;
using N4Core.Reflection.Utils.Bases;
using N4Core.Repositories.Bases;
using N4Core.Session.Utils.Bases;

namespace N4Core.Files.Services
{
    public class FileBrowserService : FileBrowserServiceBase
	{
		public FileBrowserService(UnitOfWorkBase unitOfWork, RepoBase<FileBrowserItem> repo, ReflectionUtilBase reflectionUtil, CultureUtilBase cultureUtil, SessionUtilBase sessionUtil,
            MapperUtilBase<FileBrowserItem, FileBrowserItemModel, FileBrowserItemModel> mapperUtil) : base(unitOfWork, repo, reflectionUtil, cultureUtil, sessionUtil, mapperUtil)
		{
		}
	}
}
