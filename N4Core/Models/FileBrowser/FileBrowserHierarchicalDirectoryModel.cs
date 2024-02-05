#nullable disable

using N4Core;

namespace N4Core.Models.FileBrowser
{
    public class FileBrowserHierarchicalDirectoryModel
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
    }
}
