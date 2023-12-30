#nullable disable

namespace N4Core.Configurations
{
    public class RecordFileServiceConfig
    {
        public string AcceptedExtensions { get; set; } = ".jpg, .jpeg, .png";
        public double AcceptedLengthInMegaBytes { get; set; } = 0.5;
        public List<string> Directories { get; set; } = new List<string>();
    }
}
