#nullable disable

using Microsoft.EntityFrameworkCore;
using N4Core.Records.Bases;
using N4Core.Responses.Bases;

namespace N4Core.Services.Bases
{
    public interface IServiceBase<TQueryModel, TCommandModel> : IDisposable where TQueryModel : Record, new() where TCommandModel : Record, new()
    {
        public IQueryable<TQueryModel> Query();
        public IQueryable<TCommandModel> QueryCommand();
        public Task<Response> Create(TCommandModel commandModel, CancellationToken cancellationToken = default);
        public Task<Response> Update(TCommandModel commandModel, CancellationToken cancellationToken = default);
        public Task<Response> Delete(int id, CancellationToken cancellationToken = default);
        public async Task<List<TQueryModel>> GetList(CancellationToken cancellationToken = default) => await Query().ToListAsync(cancellationToken);
        public async Task<TQueryModel> GetItem(int id, CancellationToken cancellationToken = default) => await Query().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
        public async Task<TCommandModel> GetCommandItem(int id, CancellationToken cancellationToken = default) => await QueryCommand().SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public interface IServiceBase<TModel> : IDisposable where TModel : Record, new()
    {
        IQueryable<TModel> Query();
        Response Add(TModel model);
        Response Update(TModel model);
        Response Delete(int id);
    }
}
