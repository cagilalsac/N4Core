#nullable disable

using Microsoft.EntityFrameworkCore;
using N4Core.Configurations;
using N4Core.Entities.Accounts;
using N4Core.Enums;
using N4Core.Managers.Bases;
using N4Core.Messages;
using N4Core.Models;
using N4Core.Models.Accounts;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Results;
using N4Core.Results.Bases;

namespace N4Core.Services.Bases
{
    public abstract class AccountServiceBase
    {
        public AccountServiceConfig Config { get; private set; }
        public AccountServiceMessages Messages { get; private set; }
        public ViewModel ViewModel { get; private set; }
        public Languages Language { get; private set; }

        protected readonly RepoBase<AccountUser> _userRepo;
        protected readonly CultureManagerBase _cultureManager;

        protected AccountServiceBase(RepoBase<AccountUser> userRepo, CultureManagerBase cultureManager)
        {
            _userRepo = userRepo;
            _cultureManager = cultureManager;
            Config = new AccountServiceConfig();
            Language = _cultureManager.GetLanguage();
            Messages = new AccountServiceMessages(Language);
            ViewModel = new ViewModel(Language);
        }

        public void Set(Action<AccountServiceConfig> config)
        {
            config.Invoke(Config);
            Language = Config.Language.HasValue ? Config.Language.Value : _cultureManager.GetLanguage();
            Messages = new AccountServiceMessages(Language);
            ViewModel = new ViewModel(Language);
        }

        public virtual Result<AccountUserModel> GetUser(string userName, string password)
        {
            var existingUser = _userRepo.Query().Include(q => q.Role).SingleOrDefault(q => q.UserName == userName && q.Password == password && q.IsActive);
            if (existingUser is null)
                return new ErrorResult<AccountUserModel>(Messages.UserNotFound);
            var userModel = new AccountUserModel()
            {
                Guid = existingUser.Guid,
                Id = existingUser.Id,
                UserName = existingUser.UserName,
                RoleNames = new List<string>() { existingUser.Role.RoleName }
            };
            return new SuccessResult<AccountUserModel>(Messages.UserFound, userModel);
        }

        public virtual Result<AccountUserModel> GetUser(AccountLoginModel model) => GetUser(model.UserName, model.Password);

        public virtual Result RegisterUser(AccountRegisterModel model)
        {
            if (_userRepo.Query().Any(q => q.UserName == model.UserName.Trim()))
                return new ErrorResult(Messages.UserFoundWithSameUserName);
            var entity = new AccountUser()
            {
                UserName = model.UserName.Trim(),
                Password = model.Password.Trim(),
                IsActive = true,
                RoleId = (int)Roles.DemoUser
            };
            _userRepo.Add(entity);
            return new SuccessResult(Messages.UserRegistered);
        }
    }
}
