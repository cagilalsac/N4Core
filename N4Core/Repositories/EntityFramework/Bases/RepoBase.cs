#nullable disable

using Microsoft.EntityFrameworkCore;
using N4Core.Managers.Bases;
using N4Core.Models.Reflection;
using N4Core.Records.Bases;
using N4Core.Repositories.Bases;
using System.Linq.Expressions;

namespace N4Core.Repositories.EntityFramework.Bases
{
    public abstract class RepoBase<TEntity> : IRepoBase<TEntity> where TEntity : Record, new()
    {
        protected readonly DbContext _db;
        protected readonly string _modifiedBy;

		public ReflectionRecordModel ReflectionRecordModel { get; private set; }

		protected RepoBase(DbContext db, ReflectionManagerBase reflectionManager, AccountManagerBase accountManager)
        {
            _db = db;
            ReflectionRecordModel = reflectionManager.GetReflectionRecordModel<TEntity>();
            _modifiedBy = accountManager.GetUser()?.UserName;
		}

        public virtual IQueryable<TEntity> Query(bool isNoTracking = false)
        {
            var query = isNoTracking ? _db.Set<TEntity>().AsNoTracking() : _db.Set<TEntity>();
            if (ReflectionRecordModel is not null && ReflectionRecordModel.HasIsDeleted)
                query = query.Where(q => (EF.Property<bool?>(q, ReflectionRecordModel.IsDeleted) ?? false) == false).AsQueryable();
            return query;
        }

        public virtual void Add(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Add(entity);
            if (save)
                Save();
        }

        public virtual void Update(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Update(entity);
            if (save)
                Save();
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate, bool save = true)
        {
            _db.Set<TEntity>().RemoveRange(Query().Where(predicate));
            if (save)
                Save();
        }

        public virtual int Save()
        {
            if (ReflectionRecordModel is not null)
            {
                foreach (var entry in _db.ChangeTracker.Entries<TEntity>())
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            if (ReflectionRecordModel.HasGuid)
                            {
                                entry.CurrentValues[ReflectionRecordModel.Guid] = Guid.NewGuid().ToString();
                            }
                            if (ReflectionRecordModel.HasModifiedBy)
                            {
                                entry.CurrentValues[ReflectionRecordModel.CreateDate] = DateTime.Now;
                                entry.CurrentValues[ReflectionRecordModel.CreatedBy] = _modifiedBy;
                            }
                            break;
                        case EntityState.Modified:
                            if (ReflectionRecordModel.HasGuid)
                            {
                                entry.Property(ReflectionRecordModel.Guid).IsModified = false;
                            }
                            if (ReflectionRecordModel.HasModifiedBy)
                            {
                                entry.Property(ReflectionRecordModel.CreateDate).IsModified = false;
                                entry.Property(ReflectionRecordModel.CreatedBy).IsModified = false;
                                entry.CurrentValues[ReflectionRecordModel.UpdateDate] = DateTime.Now;
                                entry.CurrentValues[ReflectionRecordModel.UpdatedBy] = _modifiedBy;
                            }
                            if (ReflectionRecordModel.HasFile && entry.CurrentValues[ReflectionRecordModel.FileContent] is not null && entry.CurrentValues[ReflectionRecordModel.FileContent].ToString() == string.Empty)
                            {
                                entry.Property(ReflectionRecordModel.FileContent).IsModified = false;
                                entry.Property(ReflectionRecordModel.FilePath).IsModified = false;
                                entry.Property(ReflectionRecordModel.FileData).IsModified = false;
                            }
                            break;
                        case EntityState.Deleted:
                            if (ReflectionRecordModel.HasGuid)
                            {
                                entry.Property(ReflectionRecordModel.Guid).IsModified = false;
                            }
                            if (ReflectionRecordModel.HasIsDeleted)
                            {
                                entry.CurrentValues[ReflectionRecordModel.IsDeleted] = true;
                                if (ReflectionRecordModel.HasModifiedBy)
                                {
                                    entry.Property(ReflectionRecordModel.CreateDate).IsModified = false;
                                    entry.Property(ReflectionRecordModel.CreatedBy).IsModified = false;
                                    entry.CurrentValues[ReflectionRecordModel.UpdateDate] = DateTime.Now;
                                    entry.CurrentValues[ReflectionRecordModel.UpdatedBy] = _modifiedBy;
                                }
                                entry.State = EntityState.Modified;
                            }
                            break;
                    }
                }
            }
            return _db.SaveChanges();
        }

        public virtual void ExecuteSql(string sql) => _db.Database.ExecuteSqlRaw(sql);

        public virtual DbSet<T> GetDbSet<T>() where T : Record, new() => _db.Set<T>();

        public void Dispose()
        {
            _db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
