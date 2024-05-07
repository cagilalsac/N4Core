#nullable disable

using AutoMapper;
using N4Core.Records.Bases;

namespace N4Core.Mappers.Utils.Bases
{
    public abstract class MapperUtilBase<TEntity, TQueryModel, TCommandModel> where TEntity : Record, new() where TQueryModel : Record, new() where TCommandModel : Record, new()
    {
        public MapperConfiguration Configuration { get; private set; } = new MapperConfiguration(c =>
        {
            c.CreateMap(typeof(TEntity), typeof(TQueryModel));
            c.CreateMap(typeof(TEntity), typeof(TCommandModel));
        });
        public List<Profile> Profiles { get; private set; }

        public void Set(params Profile[] profiles)
        {
            if (profiles is not null)
            {
                Profiles = profiles.ToList();
                Configuration = new MapperConfiguration(c =>
                {
                    c.CreateMap(typeof(TEntity), typeof(TQueryModel));
                    c.CreateMap(typeof(TEntity), typeof(TCommandModel));
                    c.AddProfiles(Profiles);
                });
            }
        }

        public virtual TQueryModel Map(TEntity entity)
        {
            MapperConfiguration configuration = new MapperConfiguration(c =>
            {
                c.CreateMap(entity.GetType(), typeof(TQueryModel));
                if (Profiles is not null)
                    c.AddProfiles(Profiles);
            });
            Mapper mapper = new Mapper(configuration);
            return mapper.Map<TQueryModel>(entity);
        }

        public virtual TEntity Map<TCommand>(TCommand commandModel)
        {
            MapperConfiguration configuration = new MapperConfiguration(c =>
            {
                c.CreateMap(commandModel.GetType(), typeof(TEntity));
                if (Profiles is not null)
                    c.AddProfiles(Profiles);
            });
            Mapper mapper = new Mapper(configuration);
            return mapper.Map<TEntity>(commandModel);
        }
    }
}
