using N4Core.Managers.Bases;
using N4Core.Settings.Bases;

namespace N4Core.Managers
{
    public class JwtManager : JwtManagerBase
    {
        public JwtManager(AppSettingsBase appSettings) : base(appSettings)
        {
        }
    }
}
