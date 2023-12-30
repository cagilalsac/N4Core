using N4Core.Records.Bases;

namespace N4Core.Services.Bases
{
    public interface IServiceBaseBase<TModel, TEntity> : IService<TModel> where TModel : RecordBase, new() where TEntity : RecordBase, new()
	{
    }
}
