namespace N4Core.Extensions
{
    public static class EnumExtension
    {
        public static Dictionary<int, string> ToDictionary(this Enum value)
        {
            return Enum.GetValues(value.GetType()).Cast<Enum>().ToDictionary(e => (int)(object)e, e => e.ToString());
        }
    }
}
