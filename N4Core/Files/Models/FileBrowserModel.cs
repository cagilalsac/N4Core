using N4Core.Files.Enums;

namespace N4Core.Files.Models
{
    public class FileBrowserModel
    {
        public List<FileBrowserItemModel>? Contents { get; set; }
        public string? Title { get; set; }
        public string? FileContent { get; set; }
        public byte[]? FileBinaryContent { get; set; }
        public FileTypes FileType { get; set; }
        public string? FileContentType { get; set; }
        public string? DirectoryLinks { get; set; }
        public bool IsStart { get; set; }
    }
}
