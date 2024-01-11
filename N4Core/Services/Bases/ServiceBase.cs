#nullable disable

using AutoMapper.QueryableExtensions;
using LinqKit;
using N4Core.Managers.Bases;
using N4Core.Models;
using N4Core.Records.Bases;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Results.Bases;
using N4Core.Utilities;
using System.Linq.Expressions;

namespace N4Core.Services.Bases
{
    public abstract class ServiceBase<TModel, TEntity> : ServiceBaseBase<TModel, TEntity> where TModel : RecordBase, new() where TEntity : RecordBase, new()
    {
        protected ServiceBase(RepoBase<TEntity> repo, ReflectionManagerBase reflectionManager, 
            CultureManagerBase cultureManager, SessionManagerBase sessionManager, 
            AccountManagerBase accountManager, RecordFileServiceBase recordFileService) 
            : base(repo, reflectionManager, cultureManager, sessionManager, accountManager, recordFileService)
        {
        }

        public override IQueryable<TModel> Query()
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
                ViewModel.OrderExpressions[i] = HelperUtil.GetDisplayName(ViewModel.OrderExpressions[i], '{', '}', ';', Config.Language);
            }
            if (_reflectionOrderingProperties is not null && _reflectionOrderingProperties.Any() && !string.IsNullOrWhiteSpace(pageOrderFilterModel.OrderExpression))
            {
                var propertyForOrdering = _reflectionOrderingProperties.FirstOrDefault(p => HelperUtil.GetDisplayName(p.DisplayName, '{', '}', ';', Config.Language) == pageOrderFilterModel.OrderExpression);
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
            UpdateImgSrc(list);
            ViewModel.TotalRecordsCount = list.Count;
            return list;
        }

        public virtual List<TModel> GetList(Expression<Func<TModel, bool>> predicate)
        {
            var list = Query().Where(predicate).ToList();
            UpdateImgSrc(list);
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
            var list = query.ToList();
            UpdateImgSrc(list);
            return list;
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
            var list = query.ToList();
            UpdateImgSrc(list);
            return list;
        }

        public virtual TModel GetItem(Expression<Func<TModel, bool>> predicate)
        {
            var item = Query().SingleOrDefault(predicate);
            UpdateImgSrc(item);
            return item;
        }

        public virtual TModel GetItem(int id)
        {
            var item = Query().SingleOrDefault(q => q.Id == id);
            UpdateImgSrc(item);
            return item;
        }

        public virtual ResultBase ItemExists(Expression<Func<TModel, bool>> predicate)
        {
            bool exists = Query().Any(predicate);
            return exists ? Success(ServiceMessages.RecordFound) : Error(ServiceMessages.RecordNotFound);
        }

        public virtual int GetMaxId()
        {
            return Query().Max(q => q.Id);
        }

        public override ResultBase Add(TModel model)
        {
            IRecordFileModel recordFileModel = null;
            IRecordFile recordFile = null;
            var entity = _mapper.Map<TEntity>(model);
            if (_repo.ReflectionRecordModel.HasFile && model is IRecordFileModel)
            {
                recordFileModel = model as IRecordFileModel;
                if (CheckFile(recordFileModel.FormFileInput) == false)
                    return Error(ServiceMessages.InvalidFileExtensionOrFileLength);
                recordFile = entity as IRecordFile;
                _recordFileService.UpdateRecordFile(recordFileModel.FormFileInput, recordFile);
            }
            _reflectionManager.TrimStringProperties(entity);
            _repo.Add(entity);
            model.Id = entity.Id;
            if (recordFileModel is not null && recordFile is not null)
            {
                UploadFile(recordFileModel.FormFileInput, recordFile);
            }
            return Success(ServiceMessages.AddedSuccessfuly);
        }

        public override ResultBase Update(TModel model)
        {
            IRecordFileModel recordFileModel = null;
            IRecordFile recordFile = null;
            var entity = _mapper.Map<TEntity>(model);
            _reflectionManager.TrimStringProperties(entity);
            if (_repo.ReflectionRecordModel.HasFile && model is IRecordFileModel)
            {
                recordFileModel = model as IRecordFileModel;
                if (CheckFile(recordFileModel.FormFileInput) == false)
                    return Error(ServiceMessages.InvalidFileExtensionOrFileLength);
                recordFile = entity as IRecordFile;
                _recordFileService.UpdateRecordFile(recordFileModel.FormFileInput, recordFile);
            }
            _repo.Update(entity);
            if (recordFileModel is not null && recordFile is not null)
            {
                UploadFile(recordFileModel.FormFileInput, recordFile);
            }
            return Success(ServiceMessages.UpdatedSuccessfuly);
        }

        public override ResultBase Delete(params int[] ids)
        {
            _repo.Delete(e => ids.Contains(e.Id));
            DeleteFiles(ids);
            return Success(ServiceMessages.DeletedSuccessfuly);
        }
    }
}