using Microsoft.AspNetCore.Http;
using N4Core.Culture.Utils.Bases;
using N4Core.Files.Utils.Bases;
using N4Core.Mappers.Utils.Bases;
using N4Core.Records.Bases;
using N4Core.Reflection.Utils.Bases;
using N4Core.Reports.Utils.Bases;
using N4Core.Repositories.Bases;
using N4Core.Services.Bases;

namespace N4Core.Services
{
    public class Service<TEntity, TQueryModel, TCommandModel> : ServiceBase<TEntity, TQueryModel, TCommandModel>
        where TEntity : Record, new() where TQueryModel : Record, new() where TCommandModel : Record, new()
    {
        public Service(UnitOfWorkBase unitOfWork, RepoBase<TEntity> repo, 
            ReflectionUtilBase reflectionUtil, CultureUtilBase cultureUtil, FileUtilBase fileUtil, ReportUtilBase reportUtil, 
            MapperUtilBase<TEntity, TQueryModel, TCommandModel> mapperUtil, IHttpContextAccessor httpContextAccessor) : 
            base(unitOfWork, repo, reflectionUtil, cultureUtil, fileUtil, reportUtil, mapperUtil, httpContextAccessor)
        {
        }
    }
}
