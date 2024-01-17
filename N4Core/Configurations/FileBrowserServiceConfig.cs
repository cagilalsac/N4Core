#nullable disable

using N4Core.Configurations.Bases;

namespace N4Core.Configurations
{
    public class FileBrowserServiceConfig : DirectoryConfig
    {
        public string Controller { get; set; } = "Home";
        public string Action { get; set; } = "Index";
        public string Area { get; set; } = "";
        public string StartLink { get; set; } = "Home";
        public byte HideAfterLevel { get; set; } = 1;
        public bool UseSession { get; set; } = true;
    }
}
