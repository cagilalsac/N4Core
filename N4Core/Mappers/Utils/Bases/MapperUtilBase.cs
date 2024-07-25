#nullable disable

using AutoMapper;
using N4Core.Records.Bases;

namespace N4Core.Mappers.Utils.Bases
{
    public abstract class MapperUtilBase<TEntity, TQueryModel, TCommandModel> where TEntity : class, IRecord, new() where TQueryModel : Record, new() where TCommandModel : Record, new()
    {
        public MapperConfiguration Configuration { get; protected set; }

        protected List<Profile> _profiles;

        protected MapperUtilBase()
        {
            Configuration = new MapperConfiguration(c =>
            {
                c.CreateMap(typeof(TEntity), typeof(TQueryModel));
                c.CreateMap(typeof(TCommandModel), typeof(TEntity));
                c.CreateMap(typeof(TEntity), typeof(TCommandModel));
            });
        }

        public void Set(params Profile[] profiles)
        {
            if (profiles is not null)
            {
                _profiles = profiles.ToList();
                Configuration = new MapperConfiguration(c =>
                {
                    c.CreateMap(typeof(TEntity), typeof(TQueryModel));
                    c.CreateMap(typeof(TCommandModel), typeof(TEntity));
                    c.CreateMap(typeof(TEntity), typeof(TCommandModel));
                    c.AddProfiles(_profiles);
                });
            }
        }

        public virtual TEntity Map(TCommandModel commandModel)
        {
            MapperConfiguration configuration = new MapperConfiguration(c =>
            {
                c.CreateMap(commandModel.GetType(), typeof(TEntity));
                if (_profiles is not null)
                    c.AddProfiles(_profiles);
            });
            Mapper mapper = new Mapper(configuration);
            return mapper.Map<TEntity>(commandModel);
        }

        public virtual TQueryModel Map(TEntity entity)
        {
            MapperConfiguration configuration = new MapperConfiguration(c =>
            {
                c.CreateMap(entity.GetType(), typeof(TQueryModel));
                if (_profiles is not null)
                    c.AddProfiles(_profiles);
            });
            Mapper mapper = new Mapper(configuration);
            return mapper.Map<TQueryModel>(entity);
        }
    }
}
