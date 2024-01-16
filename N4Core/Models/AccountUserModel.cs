#nullable disable

using N4Core.Records.Bases;

namespace N4Core.Models
{
    public class AccountUserModel : Record
    {
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string Role => Roles?.FirstOrDefault();
    }
}
