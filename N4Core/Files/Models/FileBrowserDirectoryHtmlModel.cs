namespace N4Core.Files.Models
{
    public class FileBrowserDirectoryHtmlModel : FileBrowserDirectoryModel
    {
        public bool IsLiTagCurrent { get; set; }
        public bool IsUlTagHidden { get; set; }
        public string? Link { get; set; }
    }
}
