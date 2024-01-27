using N4Core.Enums;

namespace N4Core.Configurations
{
    public class TreeNodeServiceConfig
    {
        public Languages Language { get; set; }
        public bool ShowDetailTexts { get; set; }
        public bool ShowAbbreviations { get; set; }
        public bool ShowOnlyActive { get; set; }

        public TreeNodeServiceConfig()
        {
            Language = Languages.English;
            ShowDetailTexts = false;
            ShowAbbreviations = false;
            ShowOnlyActive = true;
        }
    }
}
