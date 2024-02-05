#nullable disable


using N4Core;

namespace N4Core.Models.Reflection
{
    public class ReflectionPropertyModel
    {
        public string Name { get; } = null!;
        public string DisplayName { get; } = null!;

        public ReflectionPropertyModel(string name, string displayName = "")
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}
