#nullable disable

using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using N4Core.Culture;
using N4Core.Culture.Utils.Bases;
using N4Core.Files.Bases;
using N4Core.Files.Models;
using N4Core.Files.Models.Bases;
using N4Core.Files.Utils.Bases;
using N4Core.Mappers.Utils.Bases;
using N4Core.Messages;
using N4Core.Records.Bases;
using N4Core.Reflection.Attributes;
using N4Core.Reflection.Models;
using N4Core.Reflection.Utils.Bases;
using N4Core.Reports.Utils.Bases;
using N4Core.Repositories.Bases;
using N4Core.Responses.Bases;
using N4Core.Services.Configs;
using N4Core.Services.Models;
using N4Core.Session.Utils;
using N4Core.Session.Utils.Bases;
using N4Core.Views.Models;
using N4Core.Views.Utils;
using System.Linq.Expressions;

namespace N4Core.Services.Bases
{
    public abstract class ServiceBase<TEntity, TQueryModel, TCommandModel> : OperationResponses, IServiceBase<TQueryModel, TCommandModel>
        where TEntity : Record, new() where TQueryModel : Record, new() where TCommandModel : Record, new() 
    {
        protected readonly UnitOfWorkBase _unitOfWork;
        protected readonly RepoBase<TEntity> _repo;
        protected readonly ReflectionUtilBase _reflectionUtil;
        protected readonly CultureUtilBase _cultureUtil;
        protected readonly FileUtilBase _fileUtil;
        protected readonly ReportUtilBase _reportUtil;
        protected readonly MapperUtilBase<TEntity, TQueryModel, TCommandModel> _mapperUtil;
        protected SessionUtilBase _sessionUtil;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected List<ReflectionPropertyModel> _reflectionOrderingProperties;
        protected List<ReflectionPropertyModel> _reflectionFilteringProperties;
        public ServiceConfig Config { get; private set; }
        public Languages Language { get; private set; }
        public ViewModel ViewModel { get; private set; }
        public OperationMessagesModel Messages { get; private set; }

        protected ServiceBase(UnitOfWorkBase unitOfWork, RepoBase<TEntity> repo, 
            ReflectionUtilBase reflectionUtil, CultureUtilBase cultureUtil, FileUtilBase fileUtil, ReportUtilBase reportUtil, 
            MapperUtilBase<TEntity, TQueryModel, TCommandModel> mapperUtil, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
            _reflectionUtil = reflectionUtil;
            _cultureUtil = cultureUtil;
            _fileUtil = fileUtil;
            _reportUtil = reportUtil;
            _mapperUtil = mapperUtil;
            _httpContextAccessor = httpContextAccessor;
            _sessionUtil = new SessionUtil(_httpContextAccessor);
            _reflectionOrderingProperties = _reflectionUtil.GetReflectionPropertyModelProperties<TQueryModel>(TagAttributes.Order);
            _reflectionFilteringProperties = _reflectionUtil.GetReflectionPropertyModelProperties<TQueryModel>(TagAttributes.StringFilter);
            Config = new ServiceConfig();
            Language = _cultureUtil.GetLanguage();
            ViewModel = new ViewModel(Language);
            Messages = new OperationMessagesModel(Language);
        }

        public void Set(Action<ServiceConfig> config)
        {
            config.Invoke(Config);
            _fileUtil.Set(Config.FileLengthInMegaBytes, Config.FileExtensions, Config.FileDirectories);
            _mapperUtil.Set(Config.MapperProfiles);
            Language = Config.Language.HasValue ? Config.Language.Value : _cultureUtil.GetLanguage();
            ViewModel = new ViewModel(Language)
            {
                PageOrderFilter = Config.PageOrderFilter,
                ListCards = Config.ListCards,
                Modal = Config.Modal,
                FileOperations = Config.FileOperations,
                ExportOperation = Config.ExportOperation,
                TimePicker = Config.TimePicker
            };
            Messages = new OperationMessagesModel(Language);
        }

        public virtual IQueryable<TQueryModel> Query()
        {
            return _repo.Query(Config.NoEntityTracking).ProjectTo<TQueryModel>(_mapperUtil.Configuration);
        }

        public virtual IQueryable<TCommandModel> QueryCommand()
        {
            return _repo.Query(Config.NoEntityTracking).ProjectTo<TCommandModel>(_mapperUtil.Configuration);
        }

        public virtual IQueryable<TQueryModel> Query(PageOrderFilterModel pageOrderFilterModel)
        {
            var query = Query();
            var pageOrderFilterSession = _sessionUtil?.Get<PageOrderFilterModel>(Config.PageOrderFilterSessionKey);
            if (Config.PageOrderFilterSession && pageOrderFilterSession is not null)
            {
                pageOrderFilterModel.PageNumber = pageOrderFilterSession.PageNumber;
                pageOrderFilterModel.RecordsPerPageCount = pageOrderFilterSession.RecordsPerPageCount;
                pageOrderFilterModel.OrderExpression = pageOrderFilterSession.OrderExpression;
                pageOrderFilterModel.OrderDirectionDescending = pageOrderFilterSession.OrderDirectionDescending;
                pageOrderFilterModel.Filter = pageOrderFilterSession.Filter;
            }
            ViewModel.OrderExpressions = _reflectionOrderingProperties is null ? new List<string>() :
                _reflectionOrderingProperties.Select(pm => !string.IsNullOrWhiteSpace(pm.DisplayName) ? pm.DisplayName : pm.Name).ToList();
            for (int i = 0; i < ViewModel.OrderExpressions.Count; i++)
            {
                ViewModel.OrderExpressions[i] = HelperUtil.GetDisplayName(ViewModel.OrderExpressions[i], '{', '}', ';', Language);
            }
            if (_reflectionOrderingProperties is not null && _reflectionOrderingProperties.Any() && !string.IsNullOrWhiteSpace(pageOrderFilterModel.OrderExpression))
            {
                var propertyForOrdering = _reflectionOrderingProperties.FirstOrDefault(p => 
                    HelperUtil.GetDisplayName(p.DisplayName, '{', '}', ';', Language) == pageOrderFilterModel.OrderExpression);
                if (propertyForOrdering is null)
                    propertyForOrdering = _reflectionOrderingProperties.FirstOrDefault(p => p.Name == pageOrderFilterModel.OrderExpression);
                if (propertyForOrdering is not null)
                {
                    query = pageOrderFilterModel.OrderDirectionDescending ? query.OrderByDescending(_reflectionUtil.GetExpression<TQueryModel>(propertyForOrdering.Name)) :
                        query.OrderBy(_reflectionUtil.GetExpression<TQueryModel>(propertyForOrdering.Name));
                }
            }
            if (!string.IsNullOrWhiteSpace(pageOrderFilterModel.Filter))
            {
                if (_reflectionFilteringProperties is not null && _reflectionFilteringProperties.Any())
                {
                    var predicate = _reflectionUtil.GetPredicateContainsExpression<TQueryModel>(_reflectionFilteringProperties[0].Name, pageOrderFilterModel.Filter);
                    for (var i = 1; i < _reflectionFilteringProperties.Count; i++)
                    {
                        predicate = predicate.Or(_reflectionUtil.GetPredicateContainsExpression<TQueryModel>(_reflectionFilteringProperties[i].Name, pageOrderFilterModel.Filter));
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

        public virtual async Task<List<TQueryModel>> GetList(CancellationToken cancellationToken = default)
        {
            var list = await Query().ToListAsync(cancellationToken);
            ViewModel.TotalRecordsCount = list.Count;
            ViewModel.Message = ViewModel.TotalRecordsCount == 0 ? Messages.RecordNotFound : ViewModel.TotalRecordsCount == 1 ?
                ViewModel.TotalRecordsCount + " " + Messages.RecordFound : ViewModel.TotalRecordsCount + " " + Messages.RecordsFound;
            if (Config.FileOperations)
                _fileUtil.UpdateImgSrc(list);
            return list;
        }

        public virtual async Task<List<TQueryModel>> GetList(Expression<Func<TQueryModel, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var list = await Query().Where(predicate).ToListAsync(cancellationToken);
            ViewModel.TotalRecordsCount = list.Count;
            ViewModel.Message = ViewModel.TotalRecordsCount == 0 ? Messages.RecordNotFound : ViewModel.TotalRecordsCount == 1 ?
                ViewModel.TotalRecordsCount + " " + Messages.RecordFound : ViewModel.TotalRecordsCount + " " + Messages.RecordsFound;
            if (Config.FileOperations)
                _fileUtil.UpdateImgSrc(list);
            return list;
        }

        public virtual async Task<List<TQueryModel>> GetList(PageOrderFilterModel pageOrderFilterModel, CancellationToken cancellationToken = default)
        {
            var query = Query(pageOrderFilterModel);
            ViewModel.Message = ViewModel.TotalRecordsCount == 0 ? Messages.RecordNotFound : ViewModel.TotalRecordsCount == 1 ?
                ViewModel.TotalRecordsCount + " " + Messages.RecordFound : ViewModel.TotalRecordsCount + " " + Messages.RecordsFound;
            if (pageOrderFilterModel.PageNumber == ViewModel.PageNumbers.LastOrDefault() + 1 && ViewModel.TotalRecordsCount % Convert.ToInt32(ViewModel.RecordsPerPageCount) == 0)
            {
                if (pageOrderFilterModel.PageNumber > 1)
                    pageOrderFilterModel.PageNumber--;
            }
            if (ViewModel.RecordsPerPageCounts is not null && ViewModel.RecordsPerPageCounts.Count > 0 && ViewModel.RecordsPerPageCount != ViewModel.RecordsPerPageCounts.LastOrDefault())
            {
                query = query.Skip((pageOrderFilterModel.PageNumber - 1) * Convert.ToInt32(ViewModel.RecordsPerPageCount)).Take(Convert.ToInt32(ViewModel.RecordsPerPageCount));
            }
            _sessionUtil?.Set(pageOrderFilterModel, Config.PageOrderFilterSessionKey);
            var list = await query.ToListAsync(cancellationToken);
            if (Config.FileOperations)
                _fileUtil.UpdateImgSrc(list);
            return list;
        }

        public virtual async Task<List<TQueryModel>> GetList(Expression<Func<TQueryModel, bool>> predicate, PageOrderFilterModel pageOrderFilterModel, CancellationToken cancellationToken = default)
        {
            var query = Query(pageOrderFilterModel).Where(predicate);
            ViewModel.Message = ViewModel.TotalRecordsCount == 0 ? Messages.RecordNotFound : ViewModel.TotalRecordsCount == 1 ?
                ViewModel.TotalRecordsCount + " " + Messages.RecordFound : ViewModel.TotalRecordsCount + " " + Messages.RecordsFound;
            if (pageOrderFilterModel.PageNumber == ViewModel.PageNumbers.LastOrDefault() + 1 && ViewModel.TotalRecordsCount % Convert.ToInt32(ViewModel.RecordsPerPageCount) == 0)
            {
                if (pageOrderFilterModel.PageNumber > 1)
                    pageOrderFilterModel.PageNumber--;
            }
            if (ViewModel.RecordsPerPageCounts is not null && ViewModel.RecordsPerPageCounts.Count > 0 && ViewModel.RecordsPerPageCount != ViewModel.RecordsPerPageCounts.LastOrDefault())
            {
                query = query.Skip((pageOrderFilterModel.PageNumber - 1) * Convert.ToInt32(ViewModel.RecordsPerPageCount)).Take(Convert.ToInt32(ViewModel.RecordsPerPageCount));
            }
            _sessionUtil?.Set(pageOrderFilterModel, Config.PageOrderFilterSessionKey);
            var list = await query.ToListAsync(cancellationToken);
            if (Config.FileOperations)
                _fileUtil.UpdateImgSrc(list);
            return list;
        }

        public virtual async Task<TQueryModel> GetItem(Expression<Func<TQueryModel, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var item = await Query().SingleOrDefaultAsync(predicate, cancellationToken);
            if (Config.FileOperations)
                _fileUtil.UpdateImgSrc(item);
            return item;
        }

        public virtual async Task<TQueryModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var item = await Query().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
            if (Config.FileOperations)
                _fileUtil.UpdateImgSrc(item);
            return item;
        }

        public virtual async Task<TCommandModel> GetCommandItem(int id, CancellationToken cancellationToken = default)
        {
            var item = await QueryCommand().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
            if (Config.FileOperations)
                _fileUtil.UpdateImgSrc(item);
            return item;
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

        public virtual async Task<Response> Create(TCommandModel commandModel, CancellationToken cancellationToken = default)
        {
            RecordFileModel fileModel = null;
            RecordFile file = null;
            var entity = _mapperUtil.Map(commandModel);
            if (Config.FileOperations && _repo.ReflectionRecordModel.HasFile && commandModel is RecordFileModel)
            {
                fileModel = commandModel as RecordFileModel;
                if (_fileUtil.CheckFile(fileModel.FormFile) == false)
                    return Error(Messages.InvalidFileExtensionOrFileLength);
                file = entity as RecordFile;
                _fileUtil.UpdateFile(fileModel.FormFile, file);
            }
            _reflectionUtil.TrimStringProperties(entity);
            _repo.Create(entity);
            await _unitOfWork.SaveAsync(cancellationToken);
            commandModel.Id = entity.Id;
            if (fileModel is not null && file is not null)
            {
                _fileUtil.UploadFile(fileModel.FormFile, file);
            }
            return Success(Messages.CreatedSuccessfuly, commandModel.Id);
        }

        public virtual async Task<Response> Update(TCommandModel commandModel, CancellationToken cancellationToken = default)
        {
            RecordFileModel fileModel = null;
            RecordFile file = null;
            var entity = _mapperUtil.Map(commandModel);
            _reflectionUtil.TrimStringProperties(entity);
            if (Config.FileOperations && _repo.ReflectionRecordModel.HasFile && commandModel is RecordFileModel)
            {
                fileModel = commandModel as RecordFileModel;
                if (_fileUtil.CheckFile(fileModel.FormFile) == false)
                    return Error(Messages.InvalidFileExtensionOrFileLength);
                file = entity as RecordFile;
                _fileUtil.UpdateFile(fileModel.FormFile, file);
            }
            _repo.Update(entity);
            try
            {
                await _unitOfWork.SaveAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Error(Messages.RecordNotFound);
            }
            if (fileModel is not null && file is not null)
            {
                _fileUtil.UploadFile(fileModel.FormFile, file);
            }
            return Success(Messages.UpdatedSuccessfuly, commandModel.Id);
        }

        public virtual async Task<Response> Delete(int id, CancellationToken cancellationToken = default)
        {
            _repo.Delete(e => e.Id == id);
            await _unitOfWork.SaveAsync(cancellationToken);
            if (Config.FileOperations)
                await DeleteFile(id, cancellationToken);
            return Success(Messages.DeletedSuccessfuly);
        }

        public virtual async Task<FileToDownloadModel> DownloadFile(int id, string fileToDownloadFileNameWithoutExtension = null, bool useOctetStreamContentType = false,
            CancellationToken cancellationToken = default)
        {
            FileToDownloadModel file = _fileUtil.GetFile(id, fileToDownloadFileNameWithoutExtension, useOctetStreamContentType);
            if (file is null)
            {
                if (_repo.ReflectionRecordModel.HasFile)
                {
                    var entity = await _repo.Query().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
                    if (entity is null)
                        return null;
                    var fileDataPropertyInfo = _reflectionUtil.GetPropertyInfo(entity, _repo.ReflectionRecordModel.FileData);
                    var fileData = fileDataPropertyInfo?.GetValue(entity);
                    var fileContentPropertyInfo = _reflectionUtil.GetPropertyInfo(entity, _repo.ReflectionRecordModel.FileContent);
                    var fileContent = fileContentPropertyInfo?.GetValue(entity);
                    file = new FileToDownloadModel()
                    {
                        FileStream = fileData is not null ? new MemoryStream((byte[])fileData) : null,
                        FileContentType = fileContent is not null ? _fileUtil.GetContentType(fileContent.ToString(), false, false) : null,
                        FileName = fileContent is not null ?
                            (string.IsNullOrWhiteSpace(fileToDownloadFileNameWithoutExtension) ? id + fileContent.ToString() : 
                                fileToDownloadFileNameWithoutExtension + fileContent.ToString())
                            : null
                    };
                }
            }
            return file;
        }

        public virtual async Task DeleteFile(int id, CancellationToken cancellationToken = default)
        {
            RecordFile file = null;
            _fileUtil.DeleteFile(id);
            var entity = _repo.Query().SingleOrDefault(e => e.Id == id);
            if (entity is not null && entity is RecordFile)
            {
                file = entity as RecordFile;
                file.FileData = null;
                file.FileContent = null;
                file.FilePath = null;
                _repo.Update(entity);
                await _unitOfWork.SaveAsync(cancellationToken);
            }
        }

        public virtual async Task ExportToExcel(string fileNameWithoutExtension)
        {
            _reportUtil.Set(Config.Language, Config.IsExcelLicenseCommercial);
            _reportUtil.ExportToExcel(await GetList(), fileNameWithoutExtension);
        }

        public virtual async Task ExportToExcel(string fileNameWithoutExtension, PageOrderFilterModel pageOrderFilterModel)
        {
            _reportUtil.Set(Config.Language, Config.IsExcelLicenseCommercial);
            _reportUtil.ExportToExcel(await GetList(pageOrderFilterModel), fileNameWithoutExtension);
        }

        public void Dispose()
        {
            _repo.Dispose();
            _unitOfWork.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
