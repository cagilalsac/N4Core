using N4Core.Enums;

namespace N4Core.Configurations
{
    public class ReportServiceConfig
    {
		public Languages Language { get; set; }
		public bool IsExcelLicenseCommercial { get; set; } = false;
        public bool PageOrderFilter { get; set; } = false;
    }
}
