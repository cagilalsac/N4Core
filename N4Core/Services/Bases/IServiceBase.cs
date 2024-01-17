using N4Core.Records.Bases;

namespace N4Core.Services.Bases
{
    public interface IServiceBase<TModel, TEntity> : IService<TModel> where TModel : Record, new() where TEntity : Record, new()
	{
    }
}
