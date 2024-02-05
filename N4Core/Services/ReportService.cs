using Microsoft.AspNetCore.Http;
using N4Core.Managers.Bases;
using N4Core.Services.Bases;

namespace N4Core.Services
{
    public class ReportService : ReportServiceBase
    {
        public ReportService(ReflectionManagerBase reflectionManager, CultureManagerBase cultureManager, IHttpContextAccessor httpContextAccessor) : base(reflectionManager, cultureManager, httpContextAccessor)
        {
        }
    }
}
