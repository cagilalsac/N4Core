using N4Core.Mappers.Utils.Bases;
using N4Core.Records.Bases;

namespace N4Core.Mappers.Utils
{
    public class MapperUtil<TEntity, TQueryModel, TCommandModel> : MapperUtilBase<TEntity, TQueryModel, TCommandModel> 
        where TEntity : class, IRecord, new() where TQueryModel : Record, new() where TCommandModel : Record, new()
    {
    }
}
