using Microsoft.AspNetCore.Razor.TagHelpers;
using N4Core.Enums;
using N4Core.Extensions;
using N4Core.Utilities;

namespace N4Core.TagHelpers.Bases
{
    public abstract class TagHelperBase : TagHelper
    {
        private const char SEPERATOR = ';';
        private const char BEGIN = '{';
        private const char END = '}';
        private const string NOTVALID = "not valid";
        private const string INVALID = "invalid";
        private const string INVALIDRESULTEN = "Invalid value!";
        private const string INVALIDRESULTTR = "Geçersiz değer!";

        protected virtual string GetDisplayName(string value, Language language)
        {
            return HelperUtil.GetDisplayName(value, BEGIN, END, SEPERATOR, language);
        }

        protected virtual string GetErrorMessage(string value, Language language)
        {
            string result = string.Empty;
            string displayName;
            string[] valueParts;
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.Contains(NOTVALID, StringComparison.OrdinalIgnoreCase) || value.Contains(INVALID, StringComparison.OrdinalIgnoreCase))
                {
                    result = language == Language.Türkçe ? INVALIDRESULTTR : INVALIDRESULTEN;
                }
                else
                {
                    if (value.GetCount(BEGIN) == 2 && value.GetCount(END) == 2 && value.GetCount(SEPERATOR) == 3)
                    {
                        displayName = value.Substring(value.IndexOf('{'), value.IndexOf('}') + 1);
                        value = value.Replace(displayName, GetDisplayName(displayName, language));
                        valueParts = value.Split(SEPERATOR);
                        if (language == Language.Türkçe)
                        {
                            result = valueParts.Last();
                        }
                        else
                        {
                            result = valueParts.First();
                        }
                    }
                    else
                    {
                        result = value;
                    }
                }
            }
            return result;
        }
    }
}
