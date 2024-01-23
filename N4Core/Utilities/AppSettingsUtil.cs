#nullable disable

using Microsoft.Extensions.Configuration;

namespace N4Core.Utilities
{
	public class AppSettingsUtil
	{
		private readonly IConfiguration _configuration;

		public AppSettingsUtil(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        public T Bind<T>(string section) where T : class, new()
        {
            T t = null;
            IConfigurationSection configurationSection = _configuration.GetSection(section);
            if (configurationSection != null)
            {
                t = new T();
                configurationSection.Bind(t);
            }
			return t;
        }

        public T Bind<T>() where T : class, new()
		{
			return Bind<T>(typeof(T).Name);
		}
    }
}
