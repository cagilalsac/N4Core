#nullable disable

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using N4Core.Enums;
using System.Globalization;

namespace N4Core.Utilities
{
    public class CultureUtil
    {
        private readonly List<CultureInfo> _cultures;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CultureUtil(Language language = Language.English)
        {
            _cultures = new List<CultureInfo>();
            if (language == Language.Turkish)
                _cultures.Add(new CultureInfo("tr-TR"));
            else
                _cultures.Add(new CultureInfo("en-US"));
        }

        public CultureUtil(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Action<RequestLocalizationOptions> AddCulture()
        {
            Action<RequestLocalizationOptions> action = options =>
            {
                options.DefaultRequestCulture = new RequestCulture(_cultures?.FirstOrDefault()?.Name);
                options.SupportedCultures = _cultures;
                options.SupportedUICultures = _cultures;
            };
            return action;
        }

        public RequestLocalizationOptions UseCulture()
        {
            RequestLocalizationOptions options = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(_cultures?.FirstOrDefault()?.Name),
                SupportedCultures = _cultures,
                SupportedUICultures = _cultures
            };
            return options;
        }

        public Language GetLanguage()
        {
            var requestCultureFeature = _httpContextAccessor.HttpContext.Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var culture = requestCultureFeature.RequestCulture.Culture;
            return culture.Name == "en-US" ? Language.English : culture.Name == "tr-TR" ? Language.Turkish : Language.None;
        }
    }
}
