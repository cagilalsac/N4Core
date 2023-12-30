using Microsoft.EntityFrameworkCore;
using N4Core.Entities.Account;

namespace N4Core.Contexts.Bases
{
	public interface IAccountContext
	{
		DbSet<AccountUser> AccountUsers { get; set; }
		DbSet<AccountRole> AccountRoles { get; set; }
	}
}
