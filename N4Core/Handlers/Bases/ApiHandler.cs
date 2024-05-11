#nullable disable

using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N4Core.Culture;
using N4Core.Mappers.Utils.Bases;
using N4Core.Messages;
using N4Core.Records.Bases;
using N4Core.Reflection.Utils.Bases;
using N4Core.Repositories.Bases;
using N4Core.Requests.Bases;
using N4Core.Requests.Enums;
using N4Core.Responses.Bases;

namespace N4Core.Handlers.Bases
{
    public abstract class ApiHandler<TEntity, TRequest, TResponse> : OperationResponses, IRequestHandler<TRequest, Response<IQueryable<TResponse>>>
        where TEntity : Record, new() where TRequest : Request, IRequest<Response<IQueryable<TResponse>>>, new() where TResponse : Record, new()
    {
        protected readonly UnitOfWorkBase _unitOfWork;
        protected readonly RepoBase<TEntity> _repo;
        protected readonly ReflectionUtilBase _reflectionUtil;
        protected readonly MapperUtilBase<TEntity, TResponse, TRequest> _mapperUtil;

        public virtual bool NoEntityTracking { get; }
        public OperationMessagesModel Messages { get; private set; }

        protected ApiHandler(UnitOfWorkBase unitOfWork, RepoBase<TEntity> repo, ReflectionUtilBase reflectionUtil, MapperUtilBase<TEntity, TResponse, TRequest> mapperUtil)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _reflectionUtil = reflectionUtil;
            _mapperUtil = mapperUtil;
            Messages = new OperationMessagesModel(Languages.English);
        }

        public virtual async Task<Response<IQueryable<TResponse>>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            if (request.Operation == RequestOperations.None)
                return Error<IQueryable<TResponse>>(Messages.RequestMethodNotConfigured);
            string message = string.Empty;
            IQueryable<TResponse> query = null;
            TEntity entity;
            switch (request.Operation)
            {
                case RequestOperations.Query:
                    _mapperUtil.Set(request.MapperProfiles);
                    query = await Task.FromResult(_repo.Query(NoEntityTracking).ProjectTo<TResponse>(_mapperUtil.Configuration));
                    break;
                case RequestOperations.Create:
                    _mapperUtil.Set(request.MapperProfiles);
                    entity = _mapperUtil.Map(request);
                    _reflectionUtil.TrimStringProperties(entity);
                    _repo.Create(entity);
                    await _unitOfWork.SaveAsync(cancellationToken);
                    message = Messages.CreatedSuccessfuly;
                    break;
                case RequestOperations.Update:
                    _mapperUtil.Set(request.MapperProfiles);
                    entity = _mapperUtil.Map(request);
                    _reflectionUtil.TrimStringProperties(entity);
                    _repo.Update(entity);
                    try
                    {
                        await _unitOfWork.SaveAsync(cancellationToken);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return Error<IQueryable<TResponse>>(Messages.RecordNotFound);
                    }
                    message = Messages.UpdatedSuccessfuly;
                    break;
                case RequestOperations.Delete:
                    _repo.Delete(r => r.Id == request.Id);
                    await _unitOfWork.SaveAsync(cancellationToken);
                    message = Messages.DeletedSuccessfuly;
                    break;
            }
            return Success(message, query);
        }
    }

    public abstract class ApiHandler<TEntity, TRequest> : ApiHandler<TEntity, TRequest, Record>
        where TEntity : Record, new() where TRequest : Request, IRequest<Response<IQueryable<Record>>>, new()
    {
        protected ApiHandler(UnitOfWorkBase unitOfWork, RepoBase<TEntity> repo, ReflectionUtilBase reflectionUtil, MapperUtilBase<TEntity, Record, TRequest> mapperUtil) 
            : base(unitOfWork, repo, reflectionUtil, mapperUtil)
        {
        }
    }
}
