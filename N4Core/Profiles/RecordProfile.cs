using AutoMapper;
using N4Core.Records.Bases;

namespace N4Core.Profiles
{
    public class RecordProfile<TEntity, TModel> : Profile where TEntity : Record, new() where TModel : Record, new()
    {
        public RecordProfile()
        {
			CreateMap<TEntity, TModel>().ReverseMap();
        }
    }
}
