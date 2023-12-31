﻿#nullable disable

using Microsoft.AspNetCore.Http;
using N4Core.Configurations;
using N4Core.Utilities;
using OfficeOpenXml;

namespace N4Core.Services.Bases
{
    public abstract class ReportServiceBase
    {
        private readonly ReflectionUtil _reflectionUtil;

        protected readonly IHttpContextAccessor _httpContextAccessor;

        public ReportServiceConfig Config { get; private set; }

        protected ReportServiceBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _reflectionUtil = new ReflectionUtil();
            Config = new ReportServiceConfig();
        }

        public void Set(Action<ReportServiceConfig> config)
        {
            config.Invoke(Config);
        }

        public virtual void ExportToExcel<TModel>(List<TModel> list, string fileNameWithoutExtension) where TModel : class, new()
        {
            var data = ConvertToByteArrayForExcel(list);
            if (data is not null && data.Length > 0)
            {
                _httpContextAccessor.HttpContext.Response.Headers.Clear();
                _httpContextAccessor.HttpContext.Response.Clear();
                _httpContextAccessor.HttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                _httpContextAccessor.HttpContext.Response.Headers.Add("content-length", data.Length.ToString());
                _httpContextAccessor.HttpContext.Response.Headers.Add("content-disposition", "attachment; filename=\"" + fileNameWithoutExtension + ".xlsx\"");
                _httpContextAccessor.HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
                _httpContextAccessor.HttpContext.Response.Body.Flush();
            }
        }

        private byte[] ConvertToByteArrayForExcel<TModel>(List<TModel> list) where TModel : class, new()
        {
            byte[] data = null;
            if (list is not null && list.Any())
            {
                var dataTable = _reflectionUtil.ConvertToDataTable(list);
                if (dataTable is not null && dataTable.Rows.Count > 0)
                {
                    ExcelPackage.LicenseContext = Config.IsExcelLicenseCommercial ? LicenseContext.Commercial : LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage();
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    excelWorksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                    excelWorksheet.Cells["A:AZ"].AutoFitColumns();
                    data = excelPackage.GetAsByteArray();
                }
            }
            return data;
        }
    }
}
