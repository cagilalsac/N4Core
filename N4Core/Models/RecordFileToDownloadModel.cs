#nullable disable

namespace N4Core.Models
{
    public class RecordFileToDownloadModel
    {
        public Stream Stream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
