#nullable disable

using N4Core;

namespace N4Core.Models.FileBrowser
{
    public class FileBrowserItemModel
    {
        public string Name { get; set; }
        public string Folders { get; set; }
        public bool IsFile { get; set; }
    }
}
