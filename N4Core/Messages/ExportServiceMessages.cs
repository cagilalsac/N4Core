#nullable disable

using N4Core.Enums;

namespace N4Core.Messages
{
    public class ExportServiceMessages
    {
        public string FileNameWithoutExtension { get; set; }
        public string ExcelWorksheetName { get; set; }
        public bool IsExcelLicenseCommercial { get; set; }

        public ExportServiceMessages(Language language = Language.English, bool isExcelLicenseCommercial = false)
        {
            FileNameWithoutExtension = language == Language.Türkçe ? DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "").Replace(" ", "_").Replace(":", "") + "_Rapor" : language == Language.English ? DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "").Replace(" ", "_").Replace(":", "") + "_Report" : "";
            ExcelWorksheetName = language == Language.Türkçe ? "Sayfa1" : language == Language.English ? "Sheet1" : "";
            IsExcelLicenseCommercial = isExcelLicenseCommercial;
        }
    }
}
