using N4Core.Entities.Account;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Services.Bases;

namespace N4Core.Services
{
    public class AccountService : AccountServiceBase
    {
        public AccountService(RepoBase<AccountUser> userRepo) : base(userRepo)
        {
        }
    }
}
