using N4Core.Entities.Accounts;
using N4Core.Managers.Bases;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Services.Bases;

namespace N4Core.Services
{
    public class AccountService : AccountServiceBase
    {
        public AccountService(RepoBase<AccountUser> userRepo, CultureManagerBase cultureManager) : base(userRepo, cultureManager)
        {
        }
    }
}
