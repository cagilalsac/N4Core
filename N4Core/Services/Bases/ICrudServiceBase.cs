#nullable disable

using N4Core.Records.Bases;
using N4Core.Responses.Bases;

namespace N4Core.Services.Bases
{
    public interface ICrudServiceBase<TQueryModel, TCommandModel> : IDisposable where TQueryModel : Record, new() where TCommandModel : Record, new()
    {
        public IQueryable<TQueryModel> Query();
        public IQueryable<TCommandModel> QueryCommand();
        public Task<Response> Create(TCommandModel commandModel, CancellationToken cancellationToken = default);
        public Task<Response> Update(TCommandModel commandModel, CancellationToken cancellationToken = default);
        public Task<Response> Delete(int id, CancellationToken cancellationToken = default);
    }

    public interface ICrudServiceBase<TModel> : IDisposable where TModel : Record, new()
    {
        public IQueryable<TModel> Query();
        public Response Create(TModel model);
        public Response Update(TModel model);
        public Response Delete(int id);
    }
}
