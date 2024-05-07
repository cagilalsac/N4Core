using Microsoft.EntityFrameworkCore;
using N4Core.Accounts.Entities;

namespace N4Core.Contexts.Bases
{
    public interface IAccountDb
    {
        public DbSet<AccountUser> AccountUsers { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
    }
}
