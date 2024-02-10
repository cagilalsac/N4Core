#nullable disable

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace N4Core.Settings.Bases
{
    public abstract class AppSettingsBase
    {
        [JsonIgnore]
        public string Name { get; protected set; }

        protected readonly IConfiguration _configuration;
        protected readonly IWebHostEnvironment _webHostEnvironment;

        protected AppSettingsBase(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            Name = "AppSettings";
        }

        public virtual AppSettingsBase Bind()
        {
            if (string.IsNullOrWhiteSpace(Name) || _configuration is null)
                return null;
            _configuration.GetSection(Name).Bind(this);
            return this;
        }

        public virtual AppSettingsBase Bind(AppSettingsBase appSettings)
        {
            if (string.IsNullOrWhiteSpace(appSettings.Name) || _configuration is null)
                return null;
            _configuration.GetSection(appSettings.Name).Bind(appSettings);
            return appSettings;
        }

        public virtual AppSettingsBase Update(AppSettingsBase appSettings)
        {
            if (string.IsNullOrWhiteSpace(appSettings.Name) || _webHostEnvironment is null)
                return null;
            string[] paths =
            [
                $@"{Path.Combine(_webHostEnvironment.ContentRootPath, "appsettings.json")}",
                $@"{Path.Combine(_webHostEnvironment.ContentRootPath, "appsettings.Development.json")}"
            ];
            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    string text = File.ReadAllText(path);
                    if (!string.IsNullOrEmpty(text))
                    {
                        var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(text);
                        if (json is not null && json.ContainsKey(appSettings.Name))
                        {
                            json[appSettings.Name] = appSettings;
                        }
                        File.WriteAllText(path, JsonConvert.SerializeObject(json, Formatting.Indented));
                    }
                }
            }
            return appSettings;
        }
    }
}
