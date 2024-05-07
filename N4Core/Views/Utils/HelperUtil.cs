using N4Core.Culture;
using N4Core.Types.Extensions;

namespace N4Core.Views.Utils
{
    public static class HelperUtil
    {
        public static string GetDisplayName(string value, char begin, char end, char seperator, Languages language)
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
                    if (language == Languages.Türkçe)
                        result = valueParts.Last();
                    else
                        result = valueParts.First();
                }
            }
            return result;
        }
    }
}
