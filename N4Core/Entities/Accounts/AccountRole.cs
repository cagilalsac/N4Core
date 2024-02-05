#nullable disable

using N4Core.Records.Bases;

namespace N4Core.Entities.Accounts
{
    public class AccountRole : Record, ISoftDelete, IModifiedBy
    {
        public string RoleName { get; set; } = null!;
        public List<AccountUser> Users { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
