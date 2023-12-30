#nullable disable

using N4Core.Records.Bases;

namespace N4Core.Models
{
    public class AccountRegisterModel : IAccount
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
