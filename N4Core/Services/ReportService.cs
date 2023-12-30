using Microsoft.AspNetCore.Http;
using N4Core.Services.Bases;

namespace N4Core.Services
{
    public class ReportService : ReportServiceBase
    {
        public ReportService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }
    }
}
