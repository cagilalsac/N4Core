using N4Core.Enums;

namespace N4Core.Models.Accounts
{
    public class AccountLoginPartialModel
    {
        public bool ShowRegister { get; set; } = true;
        public bool ShowLoginLogout { get; set; } = true;
        public Languages Language { get; set; }
    }
}
