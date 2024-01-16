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
        public string UserFoundWithSameUserName { get; set; }
        public string UserLoggedIn { get; set; }
        public string UserAccessDenied { get; set; }

        public AccountServiceMessages(Language language = Language.English)
        {
            UserNotFound = language == Language.Türkçe ? "Kullanıcı bulunamadı!" : "User not found!";
            UserFound = language == Language.Türkçe ? "Kullanıcı bulundu." : "User found.";
            UserRegistered = language == Language.Türkçe ? "Kullanıcı kaydedildi." : "User registered.";
            RoleNotFound = language == Language.Türkçe ? "Rol bulunamadı!" : "Role not found!";
            UserFoundWithSameUserName = language == Language.Türkçe ? "Aynı kullanıcı adına sahip kullanıcı bulunmaktadır!" : "User with the same user name exists!";
            UserLoggedIn = language == Language.Türkçe ? "Kullanıcı girişi yapıldı." : "User logged in.";
            UserAccessDenied = language == Language.Türkçe ? "Bu kaynağa erişiminiz bulunmamaktadır!" : "You do not have access to this resource!";
        }
    }
}
