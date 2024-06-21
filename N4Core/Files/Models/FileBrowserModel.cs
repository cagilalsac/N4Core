using N4Core.Files.Enums;
using N4Core.Services.Models;

namespace N4Core.Files.Models
{
    public class FileBrowserModel : PageModel
    {
        public List<FileBrowserItemModel>? Contents { get; set; }
        public string? Title { get; set; }
        public string? FileContent { get; set; }
        public byte[]? FileBinaryContent { get; set; }
        public FileTypes FileType { get; set; }
        public string? FileContentType { get; set; }
        public string? HierarchicalDirectoryLinks { get; set; }
        public FileBrowserOperations Operation { get; set; }
        public string? OperationMessage { get; set; }
        public List<FileBrowserItemModel>? FilteredItems { get; set; }
        public string? Path { get; set; }
        public string? PlaceHolder { get; set; }
        public string Expression { get; set; } = string.Empty;
        public bool MatchCase { get; set; }
        public bool MatchWord { get; set; }
        public bool Find { get; set; }
        public bool HasExpression => !string.IsNullOrWhiteSpace(Expression);
        public bool HasFileContent => !string.IsNullOrWhiteSpace(FileContent);
        public bool HasContents => Contents is not null && Contents.Any();
    }
}
