using N4Core.Results.Bases;

namespace N4Core.Services.Bases
{
    public interface IService<TModel> : IDisposable where TModel : class, new()
	{
		IQueryable<TModel> Query();

		ResultBase Add(TModel model);

		ResultBase Update(TModel model);

		ResultBase Delete(params int[] ids);
	}
}
