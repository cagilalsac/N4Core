#nullable disable

using N4Core.Enums;
using System.Globalization;

namespace N4Core.Managers.Bases
{
    public abstract class CultureManagerBase
    {
        protected List<CultureInfo> _cultures = new List<CultureInfo>()
        {
            new CultureInfo("en-US"),
            new CultureInfo("tr-TR")
        };

        public virtual Languages GetLanguage()
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            return currentCulture.Name == _cultures[0].Name ? Languages.English : Languages.Türkçe;
        }

        public virtual CultureInfo GetCulture(string language)
        {
            return string.IsNullOrWhiteSpace(language) || language == ((int)Languages.English).ToString() ? _cultures[0] : _cultures[1];
        }
    }
}
