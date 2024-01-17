#nullable disable

using N4Core.Configurations.Bases;

namespace N4Core.Configurations
{
    public class RecordFileServiceConfig : DirectoryConfig
    {
        public string AcceptedExtensions { get; set; } = ".jpg, .jpeg, .png";
        public double AcceptedLengthInMegaBytes { get; set; } = 0.5;
    }
}
