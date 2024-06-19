using N4Core.Culture;

namespace N4Core.Types.Extensions
{
    public static class StringExtensions
    {
        public static string FirstLetterToUpperOthersToLower(this string value)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                result = value.Substring(0, 1).ToUpper();
                if (value.Length > 1)
                    result += value.Substring(1).ToLower();
            }
            return result;
        }

        public static string RemoveHtmlTags(this string value, string brTagSeperator = ", ")
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = value.Replace("&nbsp;", " ").Replace("<br>", brTagSeperator).Replace("<br />", brTagSeperator).Replace("<br/>", brTagSeperator)
                    .Replace("&amp;", "&").Trim();
                char[] array = new char[value.Length];
                int arrayIndex = 0;
                bool inside = false;
                for (int i = 0; i < value.Length; i++)
                {
                    char let = value[i];
                    if (let == '<')
                    {
                        inside = true;
                        continue;
                    }
                    if (let == '>')
                    {
                        inside = false;
                        continue;
                    }
                    if (!inside)
                    {
                        array[arrayIndex] = let;
                        arrayIndex++;
                    }
                }
                result = new string(array, 0, arrayIndex);
            }
            return result;
        }

        public static string ChangeTurkishCharactersToEnglish(this string turkishValue)
        {
            string englishValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(turkishValue))
            {
                Dictionary<string, string> characterDictionary = new Dictionary<string, string>()
                {
                    { "Ö", "O" },
                    { "Ç", "C" },
                    { "Ş", "S" },
                    { "Ğ", "G" },
                    { "Ü", "U" },
                    { "ö", "o" },
                    { "ç", "c" },
                    { "ş", "s" },
                    { "ğ", "g" },
                    { "ü", "u" },
                    { "İ", "I" },
                    { "ı", "i" }
                };
                foreach (char character in turkishValue)
                {
                    if (characterDictionary.ContainsKey(character.ToString()))
                        englishValue += characterDictionary[character.ToString()];
                    else
                        englishValue += character.ToString();
                }
            }
            return englishValue;
        }

        public static int GetCount(this string value, char character)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return value.Count(v => v == character);
            return 0;
        }

        public static string GetDisplayName(this string value, Languages language)
        {
            string result = string.Empty;
            string[] valueParts;
            if (!string.IsNullOrWhiteSpace(value))
            {
                result = value;
                if (value.GetCount('{') == 1 && value.GetCount('}') == 1 && value.GetCount(';') == 1)
                {
                    value = value.Substring(1, value.Length - 2);
                    valueParts = value.Split(';');
                    if (language == Languages.Türkçe)
                        result = valueParts.Last();
                    else
                        result = valueParts.First();
                }
            }
            return result;
        }

        public static string GetErrorMessage(this string value, Languages language)
        {
            string result = string.Empty;
            string displayName;
            string[] valueParts;
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.Contains("not valid", StringComparison.OrdinalIgnoreCase) || value.Contains("invalid", StringComparison.OrdinalIgnoreCase))
                {
                    result = language == Languages.Türkçe ? "Geçersiz değer!" : "Invalid value!";
                }
                else
                {
                    if (value.GetCount('{') == 0 && value.GetCount('}') == 0 && value.GetCount(';') == 1)
                    {
                        valueParts = value.Split(';');
                        if (language == Languages.Türkçe)
                        {
                            result = valueParts.Last();
                        }
                        else
                        {
                            result = valueParts.First();
                        }
                    }
                    else if (value.GetCount('{') == 2 && value.GetCount('}') == 2 && value.GetCount(';') == 3)
                    {
                        displayName = value.Substring(value.IndexOf('{'), value.IndexOf('}') + 1);
                        value = value.Replace(displayName, GetDisplayName(displayName, language));
                        valueParts = value.Split(';');
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

        public static string ReplaceNewLineWithLineBreak(this string value, string newLine = "\n")
        {
            string result = string.Empty;
            if (string.IsNullOrWhiteSpace(value))
                return result;
            result = value.Replace(newLine, "<br>");
            return result;
        }

        public static string SeperatePascalCaseCharacters(this string value, char seperator = ' ')
        {
            string result = string.Empty;
            if (string.IsNullOrWhiteSpace(value))
                return result;
            result += value[0];
            for (int i = 1; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]) && char.IsLetter(value[i - 1]))
                    result += seperator;
                result += value[i];
            }
            return result;
        }

        public static string? Find(this string value, string expression, string foundPrefix = "^~", string foundSuffix = "~^", string lineSeperator = "\n")
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(expression))
                return null;
            bool found = false;
            int j;
            string line, subLine;
            StringComparison comparison = StringComparison.CurrentCultureIgnoreCase;
            string[] lines = value.Split(lineSeperator);
            for (int l = 0; l < lines.Length; l++)
            {
                if (!string.IsNullOrWhiteSpace(lines[l]) && lines[l].Contains(expression, comparison))
                {
                    line = lines[l];
                    j = 0;
                    for (int i = 0; i <= line.Length - expression.Length; i++)
                    {
                        subLine = line.Substring(i, expression.Length);
                        if (subLine.Equals(expression, comparison))
                        {
                            lines[l] = lines[l].Insert(i + j, foundPrefix).Insert(i + j + expression.Length + foundPrefix.Length, foundSuffix);
                            j += foundPrefix.Length + foundSuffix.Length;
                            found = true;
                        }
                    }
                }
            }
            return found ? string.Join(lineSeperator, lines) : null;
        }
    }
}
