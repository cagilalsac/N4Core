using N4Core.Records.Bases;
using N4Core.Results.Bases;

namespace N4Core.Services.Bases
{
    public interface IService<TModel> : IDisposable where TModel : Record, new()
	{
		IQueryable<TModel> Query();

		Result Add(TModel model);

		Result Update(TModel model);

		Result Delete(params int[] ids);
	}
}
