using N4Core.JsonWebToken.Utils.Bases;
using N4Core.Settings.Bases;

namespace N4Core.JsonWebToken.Utils
{
    public class JwtUtil : JwtUtilBase
    {
        public JwtUtil(AppSettingsBase appSettings) : base(appSettings)
        {
        }
    }
}
