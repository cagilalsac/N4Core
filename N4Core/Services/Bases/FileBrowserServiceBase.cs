#nullable disable

using N4Core.Configurations;
using N4Core.Enums;
using N4Core.Managers.Bases;
using N4Core.Models.FileBrowser;
using System.Text;

namespace N4Core.Services.Bases
{
    public abstract class FileBrowserServiceBase
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

        protected readonly SessionManagerBase _sessionManager;

        public FileBrowserServiceConfig Config { get; private set; }

        protected FileBrowserServiceBase(SessionManagerBase sessionManager)
        {
            _sessionManager = sessionManager;
            Config = new FileBrowserServiceConfig();
            _routePath = Config.Path;
        }

        public void Set(Action<FileBrowserServiceConfig> config)
        {
            config.Invoke(Config);
            _routePath = Config.Path;
            _aTagHref = "href=\"" + (string.IsNullOrWhiteSpace(Config.Area) ? "/" : "/" + Config.Area + "/") + Config.Controller + "/" + Config.Action + "?path";
        }

        public virtual FileBrowserViewModel GetContents(string path, int? sectionId = null, bool includeLineNumbers = false)
        {
            if (string.IsNullOrWhiteSpace(_routePath))
                return null;
            _fullPath = _routePath + "\\";
            if (sectionId.HasValue)
                _fullPath += sectionId.Value + "\\";
            if (path is not null && path.StartsWith(Config.StartLink))
                _fullPath += path.Remove(0, Config.StartLink.Length).TrimStart('\\');
            FileBrowserViewModel fileBrowserViewModel;
            if (System.IO.File.Exists(_fullPath))
            {
                fileBrowserViewModel = GetFile(_fullPath, includeLineNumbers);
                if (fileBrowserViewModel is not null)
                {
                    if (!string.IsNullOrWhiteSpace(fileBrowserViewModel.FileContent))
                        fileBrowserViewModel.Title = AddLinks(path ?? Config.StartLink, sectionId);
                    else
                        fileBrowserViewModel.Title = System.IO.Path.GetFileName(_fullPath);
                }
            }
            else
            {
                fileBrowserViewModel = GetDirectories(_fullPath);
                var files = System.IO.Directory.GetFiles(_fullPath).OrderBy(f => f).Select(f => new FileBrowserItemModel()
                {
                    Name = System.IO.Path.GetFileName(f),
                    Folders = Config.StartLink + "\\" + f.Substring(f.IndexOf(_routePath) + _routePath.Length),
                    IsFile = true
                }).ToList();
                fileBrowserViewModel.Contents?.AddRange(files);
                fileBrowserViewModel.Title = AddLinks(path ?? Config.StartLink, sectionId);
            }
            if (fileBrowserViewModel is not null)
            {
                fileBrowserViewModel.HierarchicalDirectoryLinks = GetHierarchicalDirectoryLinks(new DirectoryInfo(_routePath), path, Config.StartLink);
                fileBrowserViewModel.IsStart = path == null || path == Config.StartLink;
            }
            return fileBrowserViewModel;
        }

        private FileBrowserViewModel GetFile(string path, bool includeLineNumbers)
        {
            string content = "";
            string line;
            int lineNumber = 0;
            FileBrowserViewModel fileBrowserViewModel = null;
            string extension = System.IO.Path.GetExtension(path).ToLower();
            if (textFiles.Keys.Contains(extension))
            {
                using (var streamReader = new System.IO.StreamReader(path, Encoding.GetEncoding(1254)))
                {
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        content += "\n" + (includeLineNumbers ? $"{++lineNumber}.\t{line}" : line);
                    }
                }
                fileBrowserViewModel = new FileBrowserViewModel()
                {
                    FileContent = content,
                    FileType = FileTypes.Text,
                    FileContentType = textFiles[extension]
                };
            }
            else if (imageFiles.Keys.Contains(extension))
            {
                fileBrowserViewModel = new FileBrowserViewModel()
                {
                    FileBinaryContent = System.IO.File.ReadAllBytes(path),
                    FileType = FileTypes.Image,
                    FileContentType = imageFiles[extension]
                };
                fileBrowserViewModel.FileContent = "data:" + imageFiles[extension] + ";base64," + Convert.ToBase64String(fileBrowserViewModel.FileBinaryContent);
            }
            else if (otherFiles.Keys.Contains(extension))
            {
                fileBrowserViewModel = new FileBrowserViewModel()
                {
                    FileBinaryContent = System.IO.File.ReadAllBytes(path),
                    FileType = FileTypes.Other,
                    FileContentType = otherFiles[extension]
                };
                fileBrowserViewModel.Title = Path.GetFileName(path);
            }
            return fileBrowserViewModel;
        }

        private FileBrowserViewModel GetDirectories(string path)
        {
            return new FileBrowserViewModel()
            {
                Contents = System.IO.Directory.GetDirectories(path).OrderBy(d => d).Select(d => new FileBrowserItemModel()
                {
                    Name = System.IO.Path.GetFileName(d),
                    Folders = Config.StartLink + "\\" + (string.IsNullOrWhiteSpace(path) ?
                    System.IO.Path.GetFileName(d) :
                    d.Substring(d.IndexOf(_routePath) + _routePath.Length))
                }).ToList()
            };
        }

        private string AddLinks(string path, int? sectionId = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                path = Config.StartLink;
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

        private string GetHierarchicalDirectoryLinks(DirectoryInfo rootDirectory, string path, string linkPath, byte level = 0)
        {
            string hierarchicalDirectoryLinksResult = "";
            List<FileBrowserHierarchicalDirectoryHtmlModel> hierarchicalDirectoriesResult;
            string sessionKey = Config.UseSession && !string.IsNullOrWhiteSpace(path) ? this.ToString()?.Split('.').LastOrDefault() + _sessionKeySuffix : "";
            List<FileBrowserHierarchicalDirectoryModel> hierarchicalDirectories = null;
            if (!string.IsNullOrWhiteSpace(sessionKey))
            {
                hierarchicalDirectories = _sessionManager?.GetSession<List<FileBrowserHierarchicalDirectoryModel>>(sessionKey);
            }
            if (hierarchicalDirectories is null)
            {
                hierarchicalDirectories = new List<FileBrowserHierarchicalDirectoryModel>();
                UpdateHierarchicalDirectories(hierarchicalDirectories, rootDirectory, path, linkPath, level);
                if (!string.IsNullOrWhiteSpace(sessionKey))
                {
                    _sessionManager?.SetSession(hierarchicalDirectories, sessionKey);
                }
            }
            if (hierarchicalDirectories is not null)
            {
                hierarchicalDirectoriesResult = hierarchicalDirectories.Select(hd => new FileBrowserHierarchicalDirectoryHtmlModel()
                {
                    Path = hd.Path,
                    Level = hd.Level,
                    Name = hd.Name,
                    IsUlTagHidden = hd.Level > Config.HideAfterLevel,
                    IsLiTagCurrent = hd.Path == UpdatePath(path)
                }).ToList();
                UpdateHtmlHierarchicalDirectories(hierarchicalDirectoriesResult);
                hierarchicalDirectoryLinksResult = $"<ul {_ulRootTagClass} {_ulRootTagStyle}>{string.Join("", hierarchicalDirectoriesResult.Select(d => d.Link))}</ul>";
            }
            return hierarchicalDirectoryLinksResult;
        }

        private void UpdateHierarchicalDirectories(List<FileBrowserHierarchicalDirectoryModel> hierarchicalDirectories, DirectoryInfo rootDirectory, string path, string linkPath, byte level = 0)
        {
            if (hierarchicalDirectories is null)
                return;
            FileBrowserHierarchicalDirectoryModel hierarchicalDirectory;
            FileBrowserHierarchicalDirectoryModel lastHierarchicalDirectory;
            DirectoryInfo[] subDirectories = rootDirectory.GetDirectories().OrderBy(d => d.Name).ToArray();
            int? lastHierarchicalDirectoryLevel;
            if (subDirectories.Length > 0)
                level++;
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                hierarchicalDirectory = new FileBrowserHierarchicalDirectoryModel();
                lastHierarchicalDirectory = hierarchicalDirectories?.LastOrDefault();
                lastHierarchicalDirectoryLevel = lastHierarchicalDirectory?.Level;
                linkPath = UpdateLinkPath(linkPath, level, lastHierarchicalDirectoryLevel, subDirectory.Name);
                hierarchicalDirectory.Path = linkPath;
                hierarchicalDirectory.Level = level;
                hierarchicalDirectory.Name = subDirectory.Name;
                hierarchicalDirectories?.Add(hierarchicalDirectory);
                UpdateHierarchicalDirectories(hierarchicalDirectories, subDirectory, path, linkPath, level);
            }
        }

        private void UpdateHtmlHierarchicalDirectories(List<FileBrowserHierarchicalDirectoryHtmlModel> hierarchicalDirectories)
        {
            int index, childIndex, childLastIndex;
            FileBrowserHierarchicalDirectoryHtmlModel hierarchicalDirectory, previousHierarchicalDirectory, childHierarchicalDirectory;
            bool currentFound, breakLoop;
            if (hierarchicalDirectories is not null)
            {
                for (index = 0; index < hierarchicalDirectories.Count; index++)
                {
                    hierarchicalDirectory = hierarchicalDirectories[index];
                    previousHierarchicalDirectory = index > 0 ? hierarchicalDirectories[index - 1] : null;
                    hierarchicalDirectory.Link = GetLink(hierarchicalDirectory, previousHierarchicalDirectory?.Level);
                    currentFound = hierarchicalDirectory.IsLiTagCurrent;
                    if (hierarchicalDirectory.Level == Config.HideAfterLevel)
                    {
                        childLastIndex = index;
                        breakLoop = false;
                        for (childIndex = index + 1; childIndex < hierarchicalDirectories.Count && !breakLoop; childIndex++)
                        {
                            childHierarchicalDirectory = hierarchicalDirectories[childIndex];
                            if (childHierarchicalDirectory.Level <= hierarchicalDirectory.Level)
                            {
                                breakLoop = true;
                            }
                            else
                            {
                                if (!currentFound)
                                    currentFound = childHierarchicalDirectory.IsLiTagCurrent;
                                childLastIndex++;
                            }
                        }
                        if (currentFound)
                        {
                            for (childIndex = index + 1; childIndex <= childLastIndex; childIndex++)
                            {
                                childHierarchicalDirectory = hierarchicalDirectories[childIndex];
                                previousHierarchicalDirectory = childIndex > 0 ? hierarchicalDirectories[childIndex - 1] : null;
                                childHierarchicalDirectory.IsUlTagHidden = false;
                                childHierarchicalDirectory.Link = GetLink(childHierarchicalDirectory, previousHierarchicalDirectory?.Level);
                            }
                            index = childLastIndex;
                        }
                    }
                }
            }
        }

        private string GetLink(FileBrowserHierarchicalDirectoryHtmlModel hierarchicalDirectory, int? lastHierarchicalDirectoryLevel)
        {
            return $"{GetUlTag(hierarchicalDirectory.Level, lastHierarchicalDirectoryLevel, hierarchicalDirectory.IsUlTagHidden)}" +
                $"<li{(hierarchicalDirectory.IsLiTagCurrent ? " " + _liClassCurrent : "")}>" +
                $"<a {(hierarchicalDirectory.IsLiTagCurrent ? _aTagStyleUnderline : _aTagStyleNone)} {_aTagHref}={hierarchicalDirectory.Path}\">" +
                $"{(hierarchicalDirectory.IsLiTagCurrent ? "<b>" + hierarchicalDirectory.Name + "</b>" : hierarchicalDirectory.Name)}</a></li>";
        }

        private string GetUlTag(int level, int? lastHierarchicalDirectoryLevel, bool isUlTagHidden)
        {
            string ulTag = "";
            if (lastHierarchicalDirectoryLevel is not null)
            {
                if (lastHierarchicalDirectoryLevel < level)
                {
                    ulTag = $"<ul {(isUlTagHidden ? _ulTagStyleHide : _ulTagStyleShow)}>";
                }
                else if (lastHierarchicalDirectoryLevel > level)
                {
                    ulTag = "</ul>";
                    for (int i = level; i < lastHierarchicalDirectoryLevel - 1; i++)
                    {
                        ulTag += "</ul>";
                    }
                }
            }
            return ulTag;
        }

        private string UpdateLinkPath(string linkPath, int level, int? lastHierarchicalDirectoryLevel, string subDirectoryName)
        {
            string[] pathItems;
            if (lastHierarchicalDirectoryLevel is null || lastHierarchicalDirectoryLevel < level)
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
