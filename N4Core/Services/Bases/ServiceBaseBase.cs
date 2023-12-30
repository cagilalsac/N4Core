#nullable disable

using AutoMapper;
using Microsoft.AspNetCore.Http;
using N4Core.Configurations;
using N4Core.Enums;
using N4Core.Messages;
using N4Core.Models;
using N4Core.Profiles;
using N4Core.Records.Bases;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Results;
using N4Core.Results.Bases;
using N4Core.Utilities;
using N4Core.Utilities.Bases;

namespace N4Core.Services.Bases
{
    public abstract class ServiceBaseBase<TModel, TEntity> : IServiceBaseBase<TModel, TEntity> where TModel : RecordBase, new() where TEntity : RecordBase, new()
    {
        protected readonly RepoBase<TEntity> _repo;
        protected readonly IReflectionUtil _reflectionUtil;
        protected readonly RecordFileServiceBase _recordFileService;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected AccountUtil _accountUtil;
        protected SessionUtil _sessionUtil;
        protected string _pageOrderFilterSessionKey = "PageOrderFilterSessionKey";
        protected Mapper _mapper;
        protected List<ReflectionPropertyModel> _reflectionOrderingProperties;
        protected List<ReflectionPropertyModel> _reflectionFilteringProperties;

        public ViewModel ViewModel { get; private set; }
        public ServiceMessages ServiceMessages { get; private set; }
        public ServiceBaseConfig Config { get; private set; }

        protected ServiceBaseBase(RepoBase<TEntity> repo, RecordFileServiceBase recordFileService, IReflectionUtil reflectionUtil, IHttpContextAccessor httpContextAccessor)
        {
            Config = new ServiceBaseConfig()
            {
                MapperConfiguration = new MapperConfiguration(c =>
                {
                    c.AddProfile(new RecordProfile<TEntity, TModel>());
                })
            };
            _mapper = new Mapper(Config.MapperConfiguration);
            _sessionUtil = new SessionUtil(httpContextAccessor);
            _httpContextAccessor = httpContextAccessor;
            _accountUtil = new AccountUtil(_httpContextAccessor);
            ServiceMessages = new ServiceMessages();
            ViewModel = new ViewModel(ServiceMessages)
            {
                PageOrderFilter = Config.PageOrderFilter,
                Modal = Config.Modal,
                FileOperations = Config.FileOperations,
                ExportOperation = Config.ExportOperation
            };
            _repo = repo;
            _recordFileService = recordFileService;
            _reflectionUtil = reflectionUtil;
			_repo.ReflectionRecordModel = _reflectionUtil.GetReflectionRecordModel<TEntity>();
			_reflectionOrderingProperties = _reflectionUtil.GetReflectionPropertyModelProperties<TModel>(TagAttribute.Order);
            _reflectionFilteringProperties = _reflectionUtil.GetReflectionPropertyModelProperties<TModel>(TagAttribute.StringFilter);
            _repo.ModifiedBy = _accountUtil.GetUser()?.UserName;
        }

        public void Set(Action<ServiceBaseConfig> config)
        {
            config.Invoke(Config);
            _mapper = new Mapper(Config.MapperConfiguration);
            ServiceMessages = new ServiceMessages(Config.Language);
            ViewModel = new ViewModel(ServiceMessages)
            {
                PageOrderFilter = Config.PageOrderFilter,
                Modal = Config.Modal,
                FileOperations = Config.FileOperations,
                ExportOperation = Config.ExportOperation
            };
            _recordFileService.Set(config =>
            {
                config.AcceptedExtensions = Config.RecordFileExtensions;
                config.AcceptedLengthInMegaBytes = Config.RecordFileLengthInMegaBytes;
                config.Directories = Config.RecordFileDirectories;
            });
        }

        public abstract IQueryable<TModel> Query();
        public abstract ResultBase Add(TModel model);
        public abstract ResultBase Update(TModel model);
        public abstract ResultBase Delete(params int[] ids);

        public virtual RecordFileToDownloadModel DownloadFile(int id, string fileToDownloadFileNameWithoutExtension = null, bool useOctetStreamContentType = false)
        {
            RecordFileToDownloadModel file = _recordFileService.GetFile(id, fileToDownloadFileNameWithoutExtension, useOctetStreamContentType);
            if (file == null)
            {
                if (_repo.ReflectionRecordModel.HasFile)
                {
                    var entity = _repo.Query().SingleOrDefault(q => q.Id == id);
                    if (entity == null)
                        return null;
                    var fileDataPropertyInfo = _reflectionUtil.GetPropertyInfo(entity, _repo.ReflectionRecordModel.FileData);
                    var fileData = fileDataPropertyInfo?.GetValue(entity);
                    var fileContentPropertyInfo = _reflectionUtil.GetPropertyInfo(entity, _repo.ReflectionRecordModel.FileContent);
                    var fileContent = fileContentPropertyInfo?.GetValue(entity);
                    file = new RecordFileToDownloadModel()
                    {
                        Stream = fileData != null ? new MemoryStream((byte[])fileData) : null,
                        ContentType = fileContent != null ? _recordFileService.GetContentType(fileContent.ToString(), false, false) : null,
                        FileName = fileContent != null ?
                            (string.IsNullOrWhiteSpace(fileToDownloadFileNameWithoutExtension) ? id + fileContent.ToString() : fileToDownloadFileNameWithoutExtension + fileContent.ToString())
                            : null
                    };
                }
            }
            return file;
        }

        public virtual void UploadFile(IFormFile formFile, IRecordFile recordFile)
        {
            _recordFileService.SaveFile(formFile, recordFile);
        }

        public virtual void DeleteFiles(params int[] ids)
        {
            IRecordFile recordFile = null;
            _recordFileService.DeleteFiles(ids);
            var entities = _repo.Query().Where(e => ids.Contains(e.Id)).ToList();
            foreach (var entity in entities)
            {
                if (entity is IRecordFile)
                {
                    recordFile = entity as IRecordFile;
                    recordFile.FileData = null;
                    recordFile.FileContent = null;
                    recordFile.FilePath = null;
					_repo.Update(entity, false);
				}
            }
            if (recordFile is not null)
                _repo.Save();
        }

        public virtual void UpdateImgSrc(TModel model)
        {
            IRecordFileModel recordFileModel;
            IRecordFile recordFile;
            if (model is IRecordFile)
            {
                recordFile = model as IRecordFile;
                recordFileModel = model as IRecordFileModel;
                recordFileModel.ImgSrcOutput = _recordFileService.GetImgSrc(recordFile);
            }
        }

        public virtual void UpdateImgSrc(List<TModel> models)
        {
            foreach (var model in models)
            {
                UpdateImgSrc(model);
            }
        }

        public virtual bool? CheckFile(IFormFile formFile)
        {
            return _recordFileService.CheckFile(formFile);
        }

        public ErrorResult Error(string message)
        {
            return new ErrorResult(message);
        }

        public ErrorResult Error()
        {
            return new ErrorResult();
        }

        public ErrorResult<TResultType> Error<TResultType>(string message, TResultType data)
        {
            return new ErrorResult<TResultType>(message, data);
        }

        public ErrorResult<TResultType> Error<TResultType>(string message)
        {
            return new ErrorResult<TResultType>(message);
        }

        public ErrorResult<TResultType> Error<TResultType>(TResultType data)
        {
            return new ErrorResult<TResultType>(data);
        }

        public ErrorResult<TResultType> Error<TResultType>()
        {
            return new ErrorResult<TResultType>();
        }

        public SuccessResult Success(string message)
        {
            return new SuccessResult(message);
        }

        public SuccessResult Success()
        {
            return new SuccessResult();
        }

        public SuccessResult<TResultType> Success<TResultType>(string message, TResultType data)
        {
            return new SuccessResult<TResultType>(message, data);
        }

        public SuccessResult<TResultType> Success<TResultType>(string message)
        {
            return new SuccessResult<TResultType>(message);
        }

        public SuccessResult<TResultType> Success<TResultType>(TResultType data)
        {
            return new SuccessResult<TResultType>(data);
        }

        public SuccessResult<TResultType> Success<TResultType>()
        {
            return new SuccessResult<TResultType>();
        }

        public void Dispose()
        {
            _repo.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}