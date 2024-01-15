#nullable disable

using AutoMapper;
using Microsoft.AspNetCore.Http;
using N4Core.Configurations;
using N4Core.Enums;
using N4Core.Managers.Bases;
using N4Core.Messages;
using N4Core.Models;
using N4Core.Profiles;
using N4Core.Records.Bases;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Results;
using N4Core.Results.Bases;
using N4Core.Texts;

namespace N4Core.Services.Bases
{
    public abstract class ServiceBaseBase<TModel, TEntity> : IServiceBaseBase<TModel, TEntity> where TModel : RecordBase, new() where TEntity : RecordBase, new()
    {
        protected readonly RepoBase<TEntity> _repo;
        protected readonly CultureManagerBase _cultureManager;
        protected readonly SessionManagerBase _sessionManager;
        protected readonly AccountManagerBase _accountManager;
        protected readonly ReflectionManagerBase _reflectionManager;
        protected readonly RecordFileServiceBase _recordFileService;
        
        protected string _pageOrderFilterSessionKey = "PageOrderFilterSessionKey";
        protected Mapper _mapper;
        protected List<ReflectionPropertyModel> _reflectionOrderingProperties;
        protected List<ReflectionPropertyModel> _reflectionFilteringProperties;

        public ViewModel ViewModel { get; private set; }
        public ViewTexts ViewTexts { get; private set; }
        public ServiceMessages ServiceMessages { get; private set; }
        public ServiceBaseConfig Config { get; private set; }

        protected ServiceBaseBase(RepoBase<TEntity> repo, ReflectionManagerBase reflectionManager, 
            CultureManagerBase cultureManager, SessionManagerBase sessionManager, 
            AccountManagerBase accountManager, RecordFileServiceBase recordFileService)
        {
            Config = new ServiceBaseConfig()
            {
                MapperConfiguration = new MapperConfiguration(c =>
                {
                    c.AddProfile(new RecordProfile<TEntity, TModel>());
                })
            };
            _mapper = new Mapper(Config.MapperConfiguration);
            _cultureManager = cultureManager;
            Config.Language = _cultureManager.GetLanguage();
            _sessionManager = sessionManager;
            _accountManager = accountManager;
            ViewModel = new ViewModel(Config.Language)
            {
                PageOrderFilter = Config.PageOrderFilter,
                Modal = Config.Modal,
                FileOperations = Config.FileOperations,
                ExportOperation = Config.ExportOperation,
                TimePicker = Config.TimePicker
            };
            _repo = repo;
            _recordFileService = recordFileService;
            _reflectionManager = reflectionManager;
            _repo.ReflectionRecordModel = _reflectionManager.GetReflectionRecordModel<TEntity>();
            _reflectionOrderingProperties = _reflectionManager.GetReflectionPropertyModelProperties<TModel>(TagAttribute.Order);
            _reflectionFilteringProperties = _reflectionManager.GetReflectionPropertyModelProperties<TModel>(TagAttribute.StringFilter);
            _repo.ModifiedBy = _accountManager.GetUser()?.UserName;
            _sessionManager = sessionManager;
            _cultureManager = cultureManager;
        }

        public void Set(Action<ServiceBaseConfig> config)
        {
            config.Invoke(Config);
            _mapper = new Mapper(Config.MapperConfiguration);
            Config.Language = _cultureManager.GetLanguage();
            ViewTexts = new ViewTexts(Config.Language);
            ServiceMessages = new ServiceMessages(Config.Language);
            ViewModel = new ViewModel(Config.Language)
            {
                PageOrderFilter = Config.PageOrderFilter,
                Modal = Config.Modal,
                FileOperations = Config.FileOperations,
                ExportOperation = Config.ExportOperation,
                TimePicker = Config.TimePicker
            };
            _recordFileService.Set(config =>
            {
                config.AcceptedExtensions = Config.FileExtensions;
                config.AcceptedLengthInMegaBytes = Config.FileLengthInMegaBytes;
                config.Directories = Config.Directories;
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
                    var fileDataPropertyInfo = _reflectionManager.GetPropertyInfo(entity, _repo.ReflectionRecordModel.FileData);
                    var fileData = fileDataPropertyInfo?.GetValue(entity);
                    var fileContentPropertyInfo = _reflectionManager.GetPropertyInfo(entity, _repo.ReflectionRecordModel.FileContent);
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