#nullable disable

using N4Core;

namespace N4Core.Models.FileBrowser
{
    public class FileBrowserHierarchicalDirectoryHtmlModel : FileBrowserHierarchicalDirectoryModel
    {
        public bool IsLiTagCurrent { get; set; }
        public bool IsUlTagHidden { get; set; }
        public string Link { get; set; }
    }
}
