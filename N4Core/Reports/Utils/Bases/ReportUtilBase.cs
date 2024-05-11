#nullable disable

using Microsoft.AspNetCore.Http;
using N4Core.Culture;
using N4Core.Culture.Utils.Bases;
using N4Core.Reflection.Utils.Bases;
using N4Core.Types.Extensions;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace N4Core.Reports.Utils.Bases
{
    public abstract class ReportUtilBase
    {
        protected readonly ReflectionUtilBase _reflectionUtil;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly CultureUtilBase _cultureUtil;
        public Languages Language { get; private set; }
        public bool IsExcelLicenseCommercial { get; private set; }

        protected ReportUtilBase(ReflectionUtilBase reflectionUtil, CultureUtilBase cultureUtil, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _cultureUtil = cultureUtil;
            _reflectionUtil = reflectionUtil;
            Language = _cultureUtil.GetLanguage();
        }

        public void Set(Languages? language, bool isExcelLicenseCommercial)
        {
            Language = language.HasValue ? language.Value : _cultureUtil.GetLanguage();
            IsExcelLicenseCommercial = isExcelLicenseCommercial;
        }

        public virtual void ExportToExcel<TModel>(List<TModel> list, string fileNameWithoutExtension) where TModel : class, new()
        {
            var data = ConvertToByteArrayForExcel(list);
            if (data is not null && data.Length > 0)
            {
                _httpContextAccessor.HttpContext.Response.Headers.Clear();
                _httpContextAccessor.HttpContext.Response.Clear();
                _httpContextAccessor.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                _httpContextAccessor.HttpContext.Response.Headers.Append("content-length", data.Length.ToString());
                _httpContextAccessor.HttpContext.Response.Headers.Append("content-disposition", "attachment; filename=\"" + fileNameWithoutExtension + ".xlsx\"");
                _httpContextAccessor.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
                _httpContextAccessor.HttpContext.Response.Body.Flush();
            }
        }

        public virtual byte[] ConvertToByteArrayForExcel<TModel>(List<TModel> list) where TModel : class, new()
        {
            byte[] data = null;
            if (list is not null && list.Any())
            {
                var dataTable = _reflectionUtil.ConvertToDataTable(list);
                if (dataTable is not null && dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        dataTable.Columns[i].ColumnName = dataTable.Columns[i].ColumnName.GetDisplayName(Language);
                    }
                    ExcelPackage.LicenseContext = IsExcelLicenseCommercial ? LicenseContext.Commercial : LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage();
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Language == Languages.English ? "Sheet1" : "Sayfa1");
                    excelWorksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    excelWorksheet.Cells["A:AZ"].AutoFitColumns();
                    data = excelPackage.GetAsByteArray();
                }
            }
            return data;
        }
    }
}
