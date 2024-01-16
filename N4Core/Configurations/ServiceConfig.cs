#nullable disable

using AutoMapper;
using N4Core.Configurations.Bases;
using N4Core.Enums;

namespace N4Core.Configurations
{
    public class ServiceConfig : DirectoryConfigBase
	{
        public bool NoTracking { get; set; } = true;
        public MapperConfiguration MapperConfiguration { get; set; } = null!;
		public Language Language { get; set; }
        public bool PageOrderFilter { get; set; } = true;
        public bool PageOrderFilterSession { get; set; } = true;
        public bool Modal { get; set; } = true;
        public bool FileOperations { get; set; }
        public bool ExportOperation { get; set; }
        public bool TimePicker { get; set; }
        public string FileExtensions { get; set; } = ".jpg, .jpeg, .png";
        public double FileLengthInMegaBytes { get; set; } = 0.5;
    }
}
