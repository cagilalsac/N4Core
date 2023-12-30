#nullable disable

using AutoMapper;
using N4Core.Enums;

namespace N4Core.Configurations
{
    public class ServiceBaseConfig
	{
        public bool NoTracking { get; set; } = true;
        public MapperConfiguration MapperConfiguration { get; set; } = null!;
		public Language Language { get; set; } = Language.English;
        public bool PageOrderFilter { get; set; } = true;
        public bool PageOrderFilterSession { get; set; } = true;
        public bool Modal { get; set; } = true;
        public bool FileOperations { get; set; }
        public bool ExportOperation { get; set; }
        public string RecordFileExtensions { get; set; } = ".jpg, .jpeg, .png";
        public double RecordFileLengthInMegaBytes { get; set; } = 0.5;
        public List<string> RecordFileDirectories { get; private set; } = new List<string>();

        public void SetRecordFileDirectories(params string[] recordFileDirectories)
		{
            RecordFileDirectories = recordFileDirectories.ToList();
		}
    }
}
