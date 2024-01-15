#nullable disable

using Microsoft.AspNetCore.Http;
using N4Core.Configurations;
using N4Core.Models;
using N4Core.Records.Bases;

namespace N4Core.Services.Bases
{
    public abstract class RecordFileServiceBase
    {
        protected char _acceptedExtensionsSeperator = ',';

        public RecordFileServiceConfig Config { get; private set; } = new RecordFileServiceConfig();

        public void Set(Action<RecordFileServiceConfig> config)
        {
            config.Invoke(Config);
        }

        public virtual string GetImgSrc(IRecordFile recordFile)
        {
            string imgSrc = "";
            if (recordFile.FileData is not null && !string.IsNullOrWhiteSpace(recordFile.FileContent))
                imgSrc = GetContentType(recordFile.FileContent) + Convert.ToBase64String(recordFile.FileData);
            else if (recordFile.Id != 0 && !string.IsNullOrWhiteSpace(recordFile.FileContent) && !string.IsNullOrWhiteSpace(recordFile.FilePath))
				imgSrc = recordFile.FilePath + recordFile.Id + recordFile.FileContent;
			return imgSrc;
        }

        public virtual void UpdateRecordFile(IFormFile formFile, IRecordFile recordFile)
        {
            recordFile.FileContent = string.Empty;
            recordFile.FilePath = null;
            recordFile.FileData = null;
            if (formFile is not null)
            {
                if (Config.Directories.Any())
                {
                    recordFile.FilePath = "/" + string.Join("/", Config.Directories) + "/";
                }
                else
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        formFile.CopyTo(memoryStream);
                        recordFile.FileData = memoryStream.ToArray();
                    }
                }
                recordFile.FileContent = Path.GetExtension(formFile.FileName).ToLower();
            }
        }

        public virtual void SaveFile(IFormFile formFile, IRecordFile recordFile)
        {
            if (formFile is not null && Config.Directories.Any())
            {
				using (FileStream fileStream = new FileStream(Path.Combine(CreatePath(recordFile.Id + recordFile.FileContent)), FileMode.Create))
				{
					formFile.CopyTo(fileStream);
				}
			}
        }

        public virtual bool? CheckFile(IFormFile formFile)
        {
            bool? result = null;
            if (formFile is not null && !string.IsNullOrWhiteSpace(Config.AcceptedExtensions))
            {
                string fileExtension = Path.GetExtension(formFile.FileName);
                string[] acceptedFileExtensionsArray = Config.AcceptedExtensions.Split(_acceptedExtensionsSeperator);
                foreach (string acceptedFileExtensionsItem in acceptedFileExtensionsArray)
                {
                    if (acceptedFileExtensionsItem.Trim().Equals(fileExtension.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        result = true;
                        break;
                    }
                }
                if (result == true)
                {
                    if (formFile.Length > Config.AcceptedLengthInMegaBytes * Math.Pow(1024, 2))
                        result = false;
                }
            }
            return result;
        }

        public virtual void DeleteFiles(params int[] ids)
        {
            var path = Config.Path;
            if (path != "")
            {
                var filePaths = Directory.GetFiles(path).Where(file => ids.Select(id => id.ToString()).Contains(Path.GetFileNameWithoutExtension(file)));
                foreach (var filePath in filePaths)
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
            }
        }

        public virtual string GetContentType(string fileNameOrExtension, bool includeData = true, bool inclueBase64 = true)
        {
            if (string.IsNullOrWhiteSpace(fileNameOrExtension))
                return "";
            Dictionary<string, string> mimeTypes = new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.ms-word" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".csv", "text/csv" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" }
            };
            string contentType;
            string fileExtension = Path.GetExtension(fileNameOrExtension).ToLower();
            contentType = mimeTypes[fileExtension];
            if (includeData)
                contentType = "data:" + contentType;
            if (inclueBase64)
                contentType = contentType + ";base64,";
            return contentType;
        }

        public virtual RecordFileToDownloadModel GetFile(int entityId, string fileToDownloadFileNameWithoutExtension = null, bool useOctetStreamContentType = false)
        {
            RecordFileToDownloadModel file = null;
            string filePath = Config.Path;
            if (filePath != "")
            {
                string fileNameWithoutPath = GetFileNameWithoutPath(entityId.ToString(), filePath);
                if (string.IsNullOrWhiteSpace(fileNameWithoutPath))
                    return null;
                string fileExtension = Path.GetExtension(fileNameWithoutPath);
                file = new RecordFileToDownloadModel()
                {
                    Stream = new FileStream(Path.Combine(filePath, fileNameWithoutPath), FileMode.Open),
                    ContentType = useOctetStreamContentType ? "application/octet-stream" : GetContentType(fileNameWithoutPath, false, false),
                    FileName = string.IsNullOrWhiteSpace(fileToDownloadFileNameWithoutExtension) ? entityId + fileExtension : fileToDownloadFileNameWithoutExtension + fileExtension
                };
            }
            return file;
        }

        private string GetFileNameWithoutPath(string fileNameWithoutExtension, string filePath)
        {
            string[] files = Directory.GetFiles(filePath);
            if (files == null || files.Length == 0)
                return null;
            string file = files.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f) == fileNameWithoutExtension);
            if (file == null)
                return null;
            return Path.GetFileName(file);
        }

        private string CreatePath(string fileName)
        {
            string path = Config.Path;
            if (path != "")
                return path + @"\" + fileName;
            return "";
        }
    }
}
