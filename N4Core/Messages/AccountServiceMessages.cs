#nullable disable

using N4Core.Enums;

namespace N4Core.Messages
{
    public class AccountServiceMessages
    {
        public string UserNotFound { get; set; }
        public string UserFound { get; set; }
        public string UserRegistered { get; set; }
        public string RoleNotFound { get; set; }

        public AccountServiceMessages(Language language = Language.English)
        {
            UserNotFound = language == Language.Turkish ? "Kullanıcı bulunamadı!" : language == Language.English ? "User not found!" : "";
            UserFound = language == Language.Turkish ? "Kullanıcı bulundu." : language == Language.English ? "User found." : "";
            UserRegistered = language == Language.Turkish ? "Kullanıcı kaydedildi." : language == Language.English ? "User registered." : "";
            RoleNotFound = language == Language.Turkish ? "Rol bulunamadı!" : language == Language.English ? "Role not found!" : "";
        }
    }
}
