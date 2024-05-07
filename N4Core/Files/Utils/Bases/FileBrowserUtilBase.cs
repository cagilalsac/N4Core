#nullable disable

using N4Core.Files.Bases;
using N4Core.Files.Enums;
using N4Core.Files.Models;
using N4Core.Session.Utils.Bases;
using System.Text;

namespace N4Core.Files.Utils.Bases
{
    public abstract class FileBrowserUtilBase : FileDirectoryBase
    {
        protected Dictionary<string, string> textFiles = new Dictionary<string, string>
        {
                { ".txt", "plaintext" },
                { ".json", "json" },
                { ".xml", "xml" },
                { ".htm", "html" },
                { ".html", "html" },
                { ".css", "css" },
                { ".js", "javascript" },
                { ".cs", "csharp" },
                { ".java", "java" },
                { ".sql", "sql" },
                { ".cshtml", "html" }
        };
        protected Dictionary<string, string> imageFiles = new Dictionary<string, string>()
        {
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" }
        };
        protected Dictionary<string, string> otherFiles = new Dictionary<string, string>()
        {
                { ".zip", "application/zip" },
                { ".7z", "application/zip" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".mp4", "video/mp4" }
        };

        protected string _routePath;
        protected string _fullPath;
        protected string _sessionKeySuffix = "DirectoriesSessionKey";
        protected string _ulRootTagClass = "class=\"directories\"";
        protected string _ulRootTagStyle = "style=\"list-style-type: none;\"";
        protected string _aTagStyleUnderline = "style=\"color: black;text-decoration: underline;font-family: Menlo,Monaco,Consolas,'Courier New',monospace !important;font-size: 16px !important;\"";
        protected string _aTagStyleNone = "style=\"color: black;text-decoration: none;font-family: Menlo,Monaco,Consolas,'Courier New',monospace !important;font-size: 16px !important;\"";
        protected string _ulTagStyleShow = "style=\"list-style-type: none;\"";
        protected string _ulTagStyleHide = "style=\"display: none;\"";
        protected string _liClassCurrent = "class=\"currentdirectory\"";
        protected string _aTagHref = "";

        protected readonly SessionUtilBase _sessionUtil;

        public string Controller { get; private set; } = "Home";
        public string Action { get; private set; } = "Index";
        public string Area { get; private set; } = "";
        public string StartLink { get; private set; } = "Home";
        public byte HideAfterLevel { get; private set; } = 1;
        public bool UseSession { get; private set; } = true;

        protected FileBrowserUtilBase(SessionUtilBase sessionUtil)
        {
            _sessionUtil = sessionUtil;
        }

        public void Set(string contoller, string action, string area, string startLink, byte hideAfterLevel, bool useSession, params string[] fileDirectories)
        {
            Controller = contoller;
            Action = action;
            Area = area;
            StartLink = startLink;
            HideAfterLevel = hideAfterLevel;
            UseSession = useSession;
            SetFileDirectories(fileDirectories);
            _routePath = FilePath;
            _aTagHref = "href=\"" + (string.IsNullOrWhiteSpace(Area) ? "/" : "/" + Area + "/") + Controller + "/" + Action + "?path";
        }

        public virtual FileBrowserModel GetContents(string path, int? sectionId = null, bool includeLineNumbers = false)
        {
            if (string.IsNullOrWhiteSpace(_routePath))
                return null;
            _fullPath = _routePath + "\\";
            if (sectionId.HasValue)
                _fullPath += sectionId.Value + "\\";
            if (path is not null && path.StartsWith(StartLink))
                _fullPath += path.Remove(0, StartLink.Length).TrimStart('\\');
            FileBrowserModel fileBrowserModel;
            if (File.Exists(_fullPath))
            {
                fileBrowserModel = GetFile(_fullPath, includeLineNumbers);
                if (fileBrowserModel is not null)
                {
                    if (!string.IsNullOrWhiteSpace(fileBrowserModel.FileContent))
                        fileBrowserModel.Title = AddLinks(path ?? StartLink, sectionId);
                    else
                        fileBrowserModel.Title = Path.GetFileName(_fullPath);
                }
            }
            else
            {
                fileBrowserModel = GetDirectories(_fullPath);
                var files = Directory.GetFiles(_fullPath).OrderBy(f => f).Select(f => new FileBrowserItemModel()
                {
                    Name = Path.GetFileName(f),
                    Folders = StartLink + "\\" + f.Substring(f.IndexOf(_routePath) + _routePath.Length),
                    IsFile = true
                }).ToList();
                fileBrowserModel.Contents?.AddRange(files);
                fileBrowserModel.Title = AddLinks(path ?? StartLink, sectionId);
            }
            if (fileBrowserModel is not null)
            {
                fileBrowserModel.DirectoryLinks = GetDirectoryLinks(new DirectoryInfo(_routePath), path, StartLink);
                fileBrowserModel.IsStart = path == null || path == StartLink;
            }
            return fileBrowserModel;
        }

        private FileBrowserModel GetFile(string path, bool includeLineNumbers)
        {
            string content = "";
            string line;
            int lineNumber = 0;
            FileBrowserModel fileBrowserModel = null;
            string extension = Path.GetExtension(path).ToLower();
            if (textFiles.Keys.Contains(extension))
            {
                using (var streamReader = new StreamReader(path, Encoding.GetEncoding(1254)))
                {
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        content += "\n" + (includeLineNumbers ? $"{++lineNumber}.\t{line}" : line);
                    }
                }
                fileBrowserModel = new FileBrowserModel()
                {
                    FileContent = content,
                    FileType = FileTypes.Text,
                    FileContentType = textFiles[extension]
                };
            }
            else if (imageFiles.Keys.Contains(extension))
            {
                fileBrowserModel = new FileBrowserModel()
                {
                    FileBinaryContent = File.ReadAllBytes(path),
                    FileType = FileTypes.Image,
                    FileContentType = imageFiles[extension]
                };
                fileBrowserModel.FileContent = "data:" + imageFiles[extension] + ";base64," + Convert.ToBase64String(fileBrowserModel.FileBinaryContent);
            }
            else if (otherFiles.Keys.Contains(extension))
            {
                fileBrowserModel = new FileBrowserModel()
                {
                    FileBinaryContent = File.ReadAllBytes(path),
                    FileType = FileTypes.Other,
                    FileContentType = otherFiles[extension]
                };
                fileBrowserModel.Title = Path.GetFileName(path);
            }
            return fileBrowserModel;
        }

        private FileBrowserModel GetDirectories(string path)
        {
            return new FileBrowserModel()
            {
                Contents = Directory.GetDirectories(path).OrderBy(d => d).Select(d => new FileBrowserItemModel()
                {
                    Name = Path.GetFileName(d),
                    Folders = StartLink + "\\" + (string.IsNullOrWhiteSpace(path) ?
                    Path.GetFileName(d) :
                    d.Substring(d.IndexOf(_routePath) + _routePath.Length))
                }).ToList()
            };
        }

        private string AddLinks(string path, int? sectionId = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = StartLink;
            if (sectionId.HasValue)
                path += "\\" + sectionId.Value;
            string[] items = path.Split('\\');
            var linkedItems = new List<string>();
            string linkedItem;
            string link;
            for (int i = 0; i < items.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(items[i]))
                {
                    linkedItem = "";
                    for (int j = 0; j <= i; j++)
                    {
                        linkedItem += items[j] + "\\";
                    }
                    link = "<a " + _aTagStyleUnderline + " " + _aTagHref + "=" + linkedItem.TrimEnd('\\') + "\">" + items[i] + "</a>";
                    linkedItems.Add(link);
                }
            }
            return string.Join("\\", linkedItems);
        }

        private string GetDirectoryLinks(DirectoryInfo rootDirectory, string path, string linkPath, byte level = 0)
        {
            string directoryLinksResult = "";
            List<FileBrowserDirectoryHtmlModel> directoriesResult;
            string sessionKey = UseSession && !string.IsNullOrWhiteSpace(path) ? this.ToString()?.Split('.').LastOrDefault() + _sessionKeySuffix : "";
            List<FileBrowserDirectoryModel> directories = null;
            if (!string.IsNullOrWhiteSpace(sessionKey))
            {
                directories = _sessionUtil?.Get<List<FileBrowserDirectoryModel>>(sessionKey);
            }
            if (directories is null)
            {
                directories = new List<FileBrowserDirectoryModel>();
                UpdateDirectories(directories, rootDirectory, path, linkPath, level);
                if (!string.IsNullOrWhiteSpace(sessionKey))
                {
                    _sessionUtil?.Set(directories, sessionKey);
                }
            }
            if (directories is not null)
            {
                directoriesResult = directories.Select(hd => new FileBrowserDirectoryHtmlModel()
                {
                    Path = hd.Path,
                    Level = hd.Level,
                    Name = hd.Name,
                    IsUlTagHidden = hd.Level > HideAfterLevel,
                    IsLiTagCurrent = hd.Path == UpdatePath(path)
                }).ToList();
                UpdateHtmlDirectories(directoriesResult);
                directoryLinksResult = $"<ul {_ulRootTagClass} {_ulRootTagStyle}>{string.Join("", directoriesResult.Select(d => d.Link))}</ul>";
            }
            return directoryLinksResult;
        }

        private void UpdateDirectories(List<FileBrowserDirectoryModel> directories, DirectoryInfo rootDirectory, string path, string linkPath, byte level = 0)
        {
            if (directories is null)
                return;
            FileBrowserDirectoryModel directory;
            FileBrowserDirectoryModel lastDirectory;
            DirectoryInfo[] subDirectories = rootDirectory.GetDirectories().OrderBy(d => d.Name).ToArray();
            int? lastDirectoryLevel;
            if (subDirectories.Length > 0)
                level++;
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                directory = new FileBrowserDirectoryModel();
                lastDirectory = directories?.LastOrDefault();
                lastDirectoryLevel = lastDirectory?.Level;
                linkPath = UpdateLinkPath(linkPath, level, lastDirectoryLevel, subDirectory.Name);
                directory.Path = linkPath;
                directory.Level = level;
                directory.Name = subDirectory.Name;
                directories?.Add(directory);
                UpdateDirectories(directories, subDirectory, path, linkPath, level);
            }
        }

        private void UpdateHtmlDirectories(List<FileBrowserDirectoryHtmlModel> directories)
        {
            int index, childIndex, childLastIndex;
            FileBrowserDirectoryHtmlModel directory, previousDirectory, childDirectory;
            bool currentFound, breakLoop;
            if (directories is not null)
            {
                for (index = 0; index < directories.Count; index++)
                {
                    directory = directories[index];
                    previousDirectory = index > 0 ? directories[index - 1] : null;
                    directory.Link = GetLink(directory, previousDirectory?.Level);
                    currentFound = directory.IsLiTagCurrent;
                    if (directory.Level == HideAfterLevel)
                    {
                        childLastIndex = index;
                        breakLoop = false;
                        for (childIndex = index + 1; childIndex < directories.Count && !breakLoop; childIndex++)
                        {
                            childDirectory = directories[childIndex];
                            if (childDirectory.Level <= directory.Level)
                            {
                                breakLoop = true;
                            }
                            else
                            {
                                if (!currentFound)
                                    currentFound = childDirectory.IsLiTagCurrent;
                                childLastIndex++;
                            }
                        }
                        if (currentFound)
                        {
                            for (childIndex = index + 1; childIndex <= childLastIndex; childIndex++)
                            {
                                childDirectory = directories[childIndex];
                                previousDirectory = childIndex > 0 ? directories[childIndex - 1] : null;
                                childDirectory.IsUlTagHidden = false;
                                childDirectory.Link = GetLink(childDirectory, previousDirectory?.Level);
                            }
                            index = childLastIndex;
                        }
                    }
                }
            }
        }

        private string GetLink(FileBrowserDirectoryHtmlModel directory, int? lastDirectoryLevel)
        {
            return $"{GetUlTag(directory.Level, lastDirectoryLevel, directory.IsUlTagHidden)}" +
                $"<li{(directory.IsLiTagCurrent ? " " + _liClassCurrent : "")}>" +
                $"<a {(directory.IsLiTagCurrent ? _aTagStyleUnderline : _aTagStyleNone)} {_aTagHref}={directory.Path}\">" +
                $"{(directory.IsLiTagCurrent ? "<b>" + directory.Name + "</b>" : directory.Name)}</a></li>";
        }

        private string GetUlTag(int level, int? lastDirectoryLevel, bool isUlTagHidden)
        {
            string ulTag = "";
            if (lastDirectoryLevel is not null)
            {
                if (lastDirectoryLevel < level)
                {
                    ulTag = $"<ul {(isUlTagHidden ? _ulTagStyleHide : _ulTagStyleShow)}>";
                }
                else if (lastDirectoryLevel > level)
                {
                    ulTag = "</ul>";
                    for (int i = level; i < lastDirectoryLevel - 1; i++)
                    {
                        ulTag += "</ul>";
                    }
                }
            }
            return ulTag;
        }

        private string UpdateLinkPath(string linkPath, int level, int? lastDirectoryLevel, string subDirectoryName)
        {
            string[] pathItems;
            if (lastDirectoryLevel is null || lastDirectoryLevel < level)
            {
                linkPath += $"\\{subDirectoryName}";
            }
            else
            {
                pathItems = linkPath.Split('\\');
                linkPath = "";
                for (int i = 0; i < pathItems.Length - 1; i++)
                {
                    linkPath += pathItems[i] + "\\";
                }
                linkPath += subDirectoryName;
            }
            return linkPath;
        }

        private string UpdatePath(string path)
        {
            string extension;
            string[] pathItems;
            if (!string.IsNullOrWhiteSpace(path))
            {
                extension = Path.GetExtension(path).ToLower();
                if (!string.IsNullOrWhiteSpace(extension))
                {
                    pathItems = path.Split('\\');
                    path = "";
                    for (int i = 0; i < pathItems.Length - 1; i++)
                    {
                        path += pathItems[i] + "\\";
                    }
                    path = path.TrimEnd('\\');
                }
            }
            return path;
        }
    }
}
