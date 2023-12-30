using AutoMapper;
using N4Core.Records.Bases;

namespace N4Core.Profiles
{
    public class RecordProfile<TEntity, TModel> : Profile where TEntity : RecordBase, new() where TModel : RecordBase, new()
    {
        public RecordProfile()
        {
			CreateMap<TEntity, TModel>().ReverseMap();
        }
    }
}
