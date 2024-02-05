#nullable disable

using N4Core.Configurations;
using N4Core.Managers.Bases;
using N4Core.Models;
using N4Core.Records.Bases;
using N4Core.Repositories.EntityFramework.Bases;
using N4Core.Results.Bases;
using System.Linq.Expressions;

namespace N4Core.Services.Bases
{
    public abstract class Service<TModel, TEntity> : ServiceBase<TModel, TEntity> where TModel : Record, new() where TEntity : Record, new()
    {
        protected readonly RecordFileServiceBase _recordFileService;

        protected Service(RepoBase<TEntity> repo, ReflectionManagerBase reflectionManager, CultureManagerBase cultureManager, SessionManagerBase sessionManager, 
            RecordFileServiceBase recordFileService) : base(repo, reflectionManager, cultureManager, sessionManager)
        {
            _recordFileService = recordFileService;
        }

        public override void Set(Action<ServiceConfig> config)
        {
            base.Set(config);
            _recordFileService.Set(config =>
            {
                config.AcceptedExtensions = Config.FileExtensions;
                config.AcceptedLengthInMegaBytes = Config.FileLengthInMegaBytes;
                config.Directories = Config.Directories;
            });
        }

        public override List<TModel> GetList()
        {
            var list = base.GetList();
            _recordFileService.UpdateImgSrc(list);
            return list;
        }

        public override List<TModel> GetList(Expression<Func<TModel, bool>> predicate)
        {
            var list = base.GetList(predicate);
            _recordFileService.UpdateImgSrc(list);
            return list;
        }

        public override List<TModel> GetList(PageOrderFilterModel pageOrderFilterModel)
        {
            var list = base.GetList(pageOrderFilterModel);
            _recordFileService.UpdateImgSrc(list);
            return list;
        }

        public override List<TModel> GetList(Expression<Func<TModel, bool>> predicate, PageOrderFilterModel pageOrderFilterModel)
        {
            var list = base.GetList(predicate, pageOrderFilterModel);
            _recordFileService.UpdateImgSrc(list);
            return list;
        }

        public override TModel GetItem(Expression<Func<TModel, bool>> predicate)
        {
            var item = base.GetItem(predicate);
            _recordFileService.UpdateImgSrc(item);
            return item;
        }

        public override TModel GetItem(int id)
        {
            var item = base.GetItem(id);
            _recordFileService.UpdateImgSrc(item);
            return item;
        }

        public override Result Add(TModel model)
        {
            IRecordFileModel recordFileModel = null;
            IRecordFile recordFile = null;
            var entity = _mapper.Map<TEntity>(model);
            if (_repo.ReflectionRecordModel.HasFile && model is IRecordFileModel)
            {
                recordFileModel = model as IRecordFileModel;
                if (_recordFileService.CheckFile(recordFileModel.FormFileInput) == false)
                    return Error(Messages.InvalidFileExtensionOrFileLength);
                recordFile = entity as IRecordFile;
                _recordFileService.UpdateRecordFile(recordFileModel.FormFileInput, recordFile);
            }
            _reflectionManager.TrimStringProperties(entity);
            _repo.Add(entity);
            model.Id = entity.Id;
            if (recordFileModel is not null && recordFile is not null)
            {
                _recordFileService.UploadFile(recordFileModel.FormFileInput, recordFile);
            }
            return Success(Messages.AddedSuccessfuly);
        }

        public override Result Update(TModel model)
        {
            IRecordFileModel recordFileModel = null;
            IRecordFile recordFile = null;
            var entity = _mapper.Map<TEntity>(model);
            _reflectionManager.TrimStringProperties(entity);
            if (_repo.ReflectionRecordModel.HasFile && model is IRecordFileModel)
            {
                recordFileModel = model as IRecordFileModel;
                if (_recordFileService.CheckFile(recordFileModel.FormFileInput) == false)
                    return Error(Messages.InvalidFileExtensionOrFileLength);
                recordFile = entity as IRecordFile;
                _recordFileService.UpdateRecordFile(recordFileModel.FormFileInput, recordFile);
            }
            _repo.Update(entity);
            if (recordFileModel is not null && recordFile is not null)
            {
                _recordFileService.UploadFile(recordFileModel.FormFileInput, recordFile);
            }
            return Success(Messages.UpdatedSuccessfuly);
        }

        public override Result Delete(int id)
        {
            var result = base.Delete(id);
            if (result.IsSuccessful)
                DeleteFile(id);
            return result;
        }

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

        public virtual void DeleteFile(int id)
        {
            IRecordFile recordFile = null;
            _recordFileService.DeleteFile(id);
            var entity = _repo.Query().SingleOrDefault(e => e.Id == id);
            if (entity is not null && entity is IRecordFile)
            {
                recordFile = entity as IRecordFile;
                recordFile.FileData = null;
                recordFile.FileContent = null;
                recordFile.FilePath = null;
                _repo.Update(entity);
            }
        }
    }
}