using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using N4Core.Enums;
using System.Globalization;

namespace N4Core.Utilities
{
	public class CultureUtil
	{
		private readonly List<CultureInfo> _cultures;

		public CultureUtil(Language language = Language.English)
		{
			_cultures = new List<CultureInfo>();
			if (language == Language.Turkish)
				_cultures.Add(new CultureInfo("tr-TR"));
			else
				_cultures.Add(new CultureInfo("en-US"));
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
	}
}
