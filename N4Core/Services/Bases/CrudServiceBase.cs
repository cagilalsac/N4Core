﻿#nullable disable

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using N4Core.Culture;
using N4Core.Culture.Utils.Bases;
using N4Core.Mappers.Utils.Bases;
using N4Core.Records.Bases;
using N4Core.Reflection.Utils.Bases;
using N4Core.Repositories.Bases;
using N4Core.Responses.Bases;
using N4Core.Responses.Managers;
using N4Core.Responses.Messages;
using N4Core.Services.Models;
using N4Core.Session.Utils.Bases;
using System.Linq.Expressions;

namespace N4Core.Services.Bases
{
    public abstract class CrudServiceBase<TEntity, TQueryModel, TCommandModel> : ResponseManager, ICrudServiceBase<TQueryModel, TCommandModel>
        where TEntity : Record, new() where TQueryModel : Record, new() where TCommandModel : Record, new()
    {
        protected readonly UnitOfWorkBase _unitOfWork;
        protected readonly RepoBase<TEntity> _repo;
        protected readonly ReflectionUtilBase _reflectionUtil;
        protected readonly CultureUtilBase _cultureUtil;
        protected readonly SessionUtilBase _sessionUtil;
        protected readonly MapperUtilBase<TEntity, TQueryModel, TCommandModel> _mapperUtil;

        protected string _pageSessionKey;
        protected bool _usePageSession;
        protected bool _noEntityTracking;
        protected string[] _recordsPerPageCounts;

        public Languages Language { get; protected set; }
        public CrudMessagesModel Messages { get; protected set; }

        protected CrudServiceBase(UnitOfWorkBase unitOfWork, RepoBase<TEntity> repo, ReflectionUtilBase reflectionUtil, CultureUtilBase cultureUtil, SessionUtilBase sessionUtil,
            MapperUtilBase<TEntity, TQueryModel, TCommandModel> mapperUtil)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _reflectionUtil = reflectionUtil;
            _cultureUtil = cultureUtil;
            _sessionUtil = sessionUtil;
            _mapperUtil = mapperUtil;
            _pageSessionKey = "PageSessionKey";
            _usePageSession = true;
            _noEntityTracking = true;
            Language = _cultureUtil.GetLanguage();
            _recordsPerPageCounts = ["5", "10", "25", "50", "100", Language == Languages.Türkçe ? "Tümü" : "All"];
            Messages = new CrudMessagesModel(Language);
        }

        public void Set(Languages? language = Languages.English, bool usePageSession = true, bool noEntityTracking = true, string[] recordsPerPageCounts = null, params Profile[] mapperProfiles)
        {
            Language = language.HasValue ? language.Value : _cultureUtil.GetLanguage();
            _usePageSession = usePageSession;
            _noEntityTracking = noEntityTracking;
            _mapperUtil.Set(mapperProfiles);
            _recordsPerPageCounts = recordsPerPageCounts is null ? ["5", "10", "25", "50", "100", Language == Languages.Türkçe ? "Tümü" : "All"] : recordsPerPageCounts;
            Messages = new CrudMessagesModel(Language);
        }

        public virtual IQueryable<TQueryModel> Query()
        {
            return _repo.Query(_noEntityTracking).ProjectTo<TQueryModel>(_mapperUtil.Configuration);
        }

        public virtual IQueryable<TQueryModel> Query(Expression<Func<TQueryModel, bool>> predicate)
        {
            return Query().Where(predicate);
        }

        public virtual IQueryable<TQueryModel> Query(Expression<Func<TQueryModel, bool>> predicate, PageModel pageModel)
        {
            var query = Query(predicate);
            return Paginate(query, pageModel);
        }

        public virtual IQueryable<TQueryModel> Query(PageModel pageModel)
        {
            var query = Query();
            return Paginate(query, pageModel);
        }

        public virtual async Task<List<TQueryModel>> GetList(CancellationToken cancellationToken = default)
        {
            return await Query().ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TQueryModel>> GetList(Expression<Func<TQueryModel, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TQueryModel>> GetList(Expression<Func<TQueryModel, bool>> predicate, PageModel pageModel, CancellationToken cancellationToken = default)
        {
            return await Query(predicate, pageModel).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TQueryModel>> GetList(PageModel pageModel, CancellationToken cancellationToken = default)
        {
            return await Query(pageModel).ToListAsync(cancellationToken);
        }

        public virtual async Task<TQueryModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            return await Query().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        public virtual async Task<Response> ItemExists(Expression<Func<TQueryModel, bool>> predicate, CancellationToken cancellationToken = default)
        {
            bool exists = await Query().AnyAsync(predicate, cancellationToken);
            return exists ? Success(Messages.RecordFound) : Error(Messages.RecordNotFound);
        }

        public virtual async Task<int> GetMaxId(CancellationToken cancellationToken = default)
        {
            return await Query().MaxAsync(q => q.Id, cancellationToken);
        }

        public virtual IQueryable<TCommandModel> QueryCommand()
        {
            return _repo.Query().ProjectTo<TCommandModel>(_mapperUtil.Configuration);
        }

        public virtual async Task<TCommandModel> GetCommandItem(int id, CancellationToken cancellationToken = default)
        {
            return await QueryCommand().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        public virtual async Task<Response> Create(TCommandModel commandModel, CancellationToken cancellationToken = default)
        {
            var entity = _mapperUtil.Map(commandModel);
            _reflectionUtil.TrimStringProperties(entity);
            _repo.Create(entity);
            await _unitOfWork.SaveAsync(cancellationToken);
            commandModel.Id = entity.Id;
            return Success(Messages.CreatedSuccessfully, commandModel.Id);
        }

        public virtual async Task<Response> Update(TCommandModel commandModel, CancellationToken cancellationToken = default)
        {
            var entity = _mapperUtil.Map(commandModel);
            _reflectionUtil.TrimStringProperties(entity);
            _repo.Update(entity);
            try
            {
                await _unitOfWork.SaveAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Error(Messages.RecordNotFound);
            }
            return Success(Messages.UpdatedSuccessfully, commandModel.Id);
        }

        public virtual async Task<Response> Delete(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repo.Query().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
            _repo.Delete(entity);
            await _unitOfWork.SaveAsync(cancellationToken);
            return Success(Messages.DeletedSuccessfully);
        }

        public virtual async Task<Response> Delete(CancellationToken cancellationToken = default)
        {
            _repo.Delete();
            await _unitOfWork.SaveAsync(cancellationToken);
            return Success(Messages.DeletedSuccessfully);
        }

        public virtual IQueryable<TQueryModel> Paginate(IQueryable<TQueryModel> query, PageModel pageModel)
        {
            pageModel.Language = Language;
            pageModel.RecordsPerPageCounts = _recordsPerPageCounts.ToList();
            if (_usePageSession && pageModel.PageSession)
            {
                var pageSessionModel = _sessionUtil.Get<PageModel>(_pageSessionKey);
                if (pageSessionModel is not null)
                {
                    pageModel.PageNumber = pageSessionModel.PageNumber;
                    pageModel.RecordsPerPageCount = pageSessionModel.RecordsPerPageCount;
                }
            }
            pageModel.TotalRecordsCount = query.Count();
            int recordsPerPageCount;
            if (pageModel.RecordsPerPageCounts is not null && pageModel.RecordsPerPageCounts.Any() && int.TryParse(pageModel.RecordsPerPageCount, out recordsPerPageCount))
                query = query.Skip((pageModel.PageNumber - 1) * recordsPerPageCount).Take(recordsPerPageCount);
            if (_usePageSession)
                _sessionUtil.Set(pageModel, _pageSessionKey);
            return query;
        }

        public virtual List<TQueryModel> Paginate(List<TQueryModel> list, PageModel pageModel)
        {
            return Paginate(list.AsQueryable(), pageModel).ToList();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _repo.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public abstract class CrudServiceBase<TEntity, TModel> : CrudServiceBase<TEntity, TModel, TModel> where TEntity : Record, new() where TModel : Record, new()
    {
        protected CrudServiceBase(UnitOfWorkBase unitOfWork, RepoBase<TEntity> repo, ReflectionUtilBase reflectionUtil, CultureUtilBase cultureUtil, SessionUtilBase sessionUtil,
            MapperUtilBase<TEntity, TModel, TModel> mapperUtil) : base(unitOfWork, repo, reflectionUtil, cultureUtil, sessionUtil, mapperUtil)
        {
        }
    }
}
