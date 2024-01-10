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

		public void Bind<T>() where T : class, new()
		{
			T t = null;
			IConfigurationSection section = _configuration.GetSection(typeof(T).Name);
			if (section != null)
			{
				t = new T();
				section.Bind(t);
			}
		}

        public void Bind<T>(string sectionKey) where T : class, new()
        {
            T t = null;
            IConfigurationSection section = _configuration.GetSection(sectionKey);
            if (section != null)
            {
                t = new T();
                section.Bind(t);
            }
        }
    }
}
