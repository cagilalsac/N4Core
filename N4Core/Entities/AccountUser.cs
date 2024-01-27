#nullable disable

using N4Core;
using N4Core.Records.Bases;

namespace N4Core.Entities
{
    public class AccountUser : Record, ISoftDelete, IModifiedBy
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public AccountRole Role { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
