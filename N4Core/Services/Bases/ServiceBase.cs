#nullable disable

using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using N4Core.Configurations;
using N4Core.Enums;
using N4Core.Managers.Bases;
using N4Core.Messages;
using N4Core.Models;
using N4Core.Models.Reflection;
using N4Core.Profiles;
using N4Core.Records.Bases;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Results.Bases;
using N4Core.Utilities;
using System.Linq.Expressions;

namespace N4Core.Services.Bases
{
    public abstract class ServiceBase<TModel, TEntity> : ServiceResult, IServiceBase<TModel, TEntity> where TModel : Record, new() where TEntity : Record, new()
    {
        protected readonly RepoBase<TEntity> _repo;
        protected readonly CultureManagerBase _cultureManager;
        protected readonly ReflectionManagerBase _reflectionManager;
        protected readonly SessionManagerBase _sessionManager;

        protected string _pageOrderFilterSessionKey = "PageOrderFilterSessionKey";
        protected List<ReflectionPropertyModel> _reflectionOrderingProperties;
        protected List<ReflectionPropertyModel> _reflectionFilteringProperties;
        protected Mapper _mapper;

        public ServiceConfig Config { get; private set; }
        public ViewModel ViewModel { get; private set; }
        public ServiceMessages Messages { get; private set; }
        public Languages Language { get; private set; }

        protected ServiceBase(RepoBase<TEntity> repo, ReflectionManagerBase reflectionManager, 
            CultureManagerBase cultureManager, SessionManagerBase sessionManager)
        {
            _repo = repo;
            _reflectionManager = reflectionManager;
            _cultureManager = cultureManager;
            _sessionManager = sessionManager;
            Config = new ServiceConfig()
            {
                MapperConfiguration = new MapperConfiguration(c =>
                {
                    c.AddProfile(new RecordProfile<TEntity, TModel>());
                }),
            };
            Language = _cultureManager.GetLanguage();
            ViewModel = new ViewModel(Language)
            {
                PageOrderFilter = Config.PageOrderFilter,
                Modal = Config.Modal,
                FileOperations = Config.FileOperations,
                ExportOperation = Config.ExportOperation,
                TimePicker = Config.TimePicker
            };
            Messages = new ServiceMessages(Language);
            _mapper = new Mapper(Config.MapperConfiguration);
            _reflectionOrderingProperties = _reflectionManager.GetReflectionPropertyModelProperties<TModel>(TagAttributes.Order);
            _reflectionFilteringProperties = _reflectionManager.GetReflectionPropertyModelProperties<TModel>(TagAttributes.StringFilter);
        }

        public virtual void Set(Action<ServiceConfig> config)
        {
            config.Invoke(Config);
            Language = Config.Language.HasValue ? Config.Language.Value : _cultureManager.GetLanguage();
            ViewModel = new ViewModel(Language)
            {
                PageOrderFilter = Config.PageOrderFilter,
                Modal = Config.Modal,
                FileOperations = Config.FileOperations,
                ExportOperation = Config.ExportOperation,
                TimePicker = Config.TimePicker
            };
            Messages = new ServiceMessages(Language);
            _mapper = new Mapper(Config.MapperConfiguration);
        }

        public virtual IQueryable<TModel> Query()
        {
            return _repo.Query(Config.NoTracking).ProjectTo<TModel>(Config.MapperConfiguration);
        }

        public virtual IQueryable<TModel> Query(PageOrderFilterModel pageOrderFilterModel)
        {
            var query = Query();
            var pageOrderFilterSession = _sessionManager?.GetSession<PageOrderFilterModel>(_pageOrderFilterSessionKey);
            if (Config.PageOrderFilterSession && pageOrderFilterSession != null)
            {
                pageOrderFilterModel.PageNumber = pageOrderFilterSession.PageNumber;
                pageOrderFilterModel.RecordsPerPageCount = pageOrderFilterSession.RecordsPerPageCount;
                pageOrderFilterModel.OrderExpression = pageOrderFilterSession.OrderExpression;
                pageOrderFilterModel.OrderDirectionDescending = pageOrderFilterSession.OrderDirectionDescending;
                pageOrderFilterModel.Filter = pageOrderFilterSession.Filter;
            }
            ViewModel.OrderExpressions = _reflectionOrderingProperties is null ?
                new List<string>() :
                _reflectionOrderingProperties
                    .Select(pm => !string.IsNullOrWhiteSpace(pm.DisplayName) ? pm.DisplayName : pm.Name).ToList();
            for (int i = 0; i < ViewModel.OrderExpressions.Count; i++)
            {
                ViewModel.OrderExpressions[i] = HelperUtil.GetDisplayName(ViewModel.OrderExpressions[i], '{', '}', ';', Language);
            }
            if (_reflectionOrderingProperties is not null && _reflectionOrderingProperties.Any() && !string.IsNullOrWhiteSpace(pageOrderFilterModel.OrderExpression))
            {
                var propertyForOrdering = _reflectionOrderingProperties.FirstOrDefault(p => HelperUtil.GetDisplayName(p.DisplayName, '{', '}', ';', Language) == pageOrderFilterModel.OrderExpression);
                if (propertyForOrdering == null)
                    propertyForOrdering = _reflectionOrderingProperties.FirstOrDefault(p => p.Name == pageOrderFilterModel.OrderExpression);
                if (propertyForOrdering != null)
                {
                    query = pageOrderFilterModel.OrderDirectionDescending ? query.OrderByDescending(_reflectionManager.GetExpression<TModel>(propertyForOrdering.Name))
                        : query.OrderBy(_reflectionManager.GetExpression<TModel>(propertyForOrdering.Name));
                }
            }
            if (!string.IsNullOrWhiteSpace(pageOrderFilterModel.Filter))
            {
                if (_reflectionFilteringProperties is not null && _reflectionFilteringProperties.Any())
                {
                    var predicate = _reflectionManager.GetPredicateContainsExpression<TModel>(_reflectionFilteringProperties[0].Name, pageOrderFilterModel.Filter);
                    for (var i = 1; i < _reflectionFilteringProperties.Count; i++)
                    {
                        predicate = predicate.Or(_reflectionManager.GetPredicateContainsExpression<TModel>(_reflectionFilteringProperties[i].Name, pageOrderFilterModel.Filter));
                    }
                    query = query.Where(predicate);
                }
            }
            ViewModel.PageNumber = pageOrderFilterModel.PageNumber;
            ViewModel.RecordsPerPageCount = pageOrderFilterModel.RecordsPerPageCount;
            ViewModel.OrderExpression = pageOrderFilterModel.OrderExpression;
            ViewModel.OrderDirectionDescending = pageOrderFilterModel.OrderDirectionDescending;
            ViewModel.Filter = pageOrderFilterModel.Filter;
            ViewModel.TotalRecordsCount = query.Count();
            return query;
        }

        public virtual List<TModel> GetList()
        {
            var list = Query().ToList();
            ViewModel.TotalRecordsCount = list.Count;
            return list;
        }

        public virtual List<TModel> GetList(Expression<Func<TModel, bool>> predicate)
        {
            var list = Query().Where(predicate).ToList();
            ViewModel.TotalRecordsCount = list.Count;
            return list;
        }

        public virtual List<TModel> GetList(PageOrderFilterModel pageOrderFilterModel)
        {
            var query = Query(pageOrderFilterModel);
            if (pageOrderFilterModel.PageNumber == ViewModel.PageNumbers.LastOrDefault() + 1 && ViewModel.TotalRecordsCount % Convert.ToInt32(ViewModel.RecordsPerPageCount) == 0)
            {
                if (pageOrderFilterModel.PageNumber > 1)
                    pageOrderFilterModel.PageNumber--;
            }
            if (ViewModel.RecordsPerPageCounts != null && ViewModel.RecordsPerPageCounts.Count > 0 && ViewModel.RecordsPerPageCount != ViewModel.RecordsPerPageCounts.LastOrDefault())
            {
                query = query.Skip((pageOrderFilterModel.PageNumber - 1) * Convert.ToInt32(ViewModel.RecordsPerPageCount)).Take(Convert.ToInt32(ViewModel.RecordsPerPageCount));
            }
            _sessionManager?.SetSession(pageOrderFilterModel, _pageOrderFilterSessionKey);
            return query.ToList();
        }

        public virtual List<TModel> GetList(Expression<Func<TModel, bool>> predicate, PageOrderFilterModel pageOrderFilterModel)
        {
            var query = Query(pageOrderFilterModel).Where(predicate);
            if (pageOrderFilterModel.PageNumber == ViewModel.PageNumbers.LastOrDefault() + 1 && ViewModel.TotalRecordsCount % Convert.ToInt32(ViewModel.RecordsPerPageCount) == 0)
            {
                if (pageOrderFilterModel.PageNumber > 1)
                    pageOrderFilterModel.PageNumber--;
            }
            if (ViewModel.RecordsPerPageCounts != null && ViewModel.RecordsPerPageCounts.Count > 0 && ViewModel.RecordsPerPageCount != ViewModel.RecordsPerPageCounts.LastOrDefault())
            {
                query = query.Skip((pageOrderFilterModel.PageNumber - 1) * Convert.ToInt32(ViewModel.RecordsPerPageCount)).Take(Convert.ToInt32(ViewModel.RecordsPerPageCount));
            }
            _sessionManager?.SetSession(pageOrderFilterModel, _pageOrderFilterSessionKey);
            return query.ToList();
        }

        public virtual TModel GetItem(Expression<Func<TModel, bool>> predicate)
        {
            return Query().SingleOrDefault(predicate);
        }

        public virtual TModel GetItem(int id)
        {
            return Query().SingleOrDefault(q => q.Id == id);
        }

        public virtual Result ItemExists(Expression<Func<TModel, bool>> predicate)
        {
            bool exists = Query().Any(predicate);
            return exists ? Success(Messages.RecordFound) : Error(Messages.RecordNotFound);
        }

        public virtual int GetMaxId()
        {
            return Query().Max(q => q.Id);
        }

        public virtual Result Add(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            _reflectionManager.TrimStringProperties(entity);
            _repo.Add(entity);
            model.Id = entity.Id;
            return Success(Messages.AddedSuccessfuly);
        }

        public virtual Result Update(TModel model)
        {
            var entity = _mapper.Map<TEntity>(model);
            _reflectionManager.TrimStringProperties(entity);
            _repo.Update(entity);
            return Success(Messages.UpdatedSuccessfuly);
        }

        public virtual Result Delete(int id)
        {
            _repo.Delete(e => e.Id == id);
            return Success(Messages.DeletedSuccessfuly);
        }

        public void Dispose()
        {
            _repo.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}