using N4Core.Enums;
using N4Core.Extensions;

namespace N4Core.Utilities
{
    public static class HelperUtil
    {
        public static string GetDisplayName(string value, char begin, char end, char seperator, Language language)
        {
            string result = string.Empty;
            string[] valueParts;
            if (!string.IsNullOrWhiteSpace(value))
            {
                result = value;
                if (value.GetCount(begin) == 1 && value.GetCount(end) == 1 && value.GetCount(seperator) == 1)
                {
                    value = value.Substring(1, value.Length - 2);
                    valueParts = value.Split(seperator);
                    if (language == Language.Türkçe)
                        result = valueParts.Last();
                    else
                        result = valueParts.First();
                }
            }
            return result;
        }
    }
}
