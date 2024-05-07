using Microsoft.AspNetCore.Razor.TagHelpers;
using N4Core.Culture;
using N4Core.Types.Extensions;
using N4Core.Views.Utils;

namespace N4Core.Views.TagHelpers.Bases
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

        protected virtual string GetDisplayName(string value, Languages language)
        {
            return HelperUtil.GetDisplayName(value, BEGIN, END, SEPERATOR, language);
        }

        protected virtual string GetErrorMessage(string value, Languages language)
        {
            string result = string.Empty;
            string displayName;
            string[] valueParts;
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.Contains(NOTVALID, StringComparison.OrdinalIgnoreCase) || value.Contains(INVALID, StringComparison.OrdinalIgnoreCase))
                {
                    result = language == Languages.Türkçe ? INVALIDRESULTTR : INVALIDRESULTEN;
                }
                else
                {
                    if (value.GetCount(BEGIN) == 0 && value.GetCount(END) == 0 && value.GetCount(SEPERATOR) == 1)
                    {
                        valueParts = value.Split(SEPERATOR);
                        if (language == Languages.Türkçe)
                        {
                            result = valueParts.Last();
                        }
                        else
                        {
                            result = valueParts.First();
                        }
                    }
                    else if (value.GetCount(BEGIN) == 2 && value.GetCount(END) == 2 && value.GetCount(SEPERATOR) == 3)
                    {
                        displayName = value.Substring(value.IndexOf('{'), value.IndexOf('}') + 1);
                        value = value.Replace(displayName, GetDisplayName(displayName, language));
                        valueParts = value.Split(SEPERATOR);
                        if (language == Languages.Türkçe)
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
