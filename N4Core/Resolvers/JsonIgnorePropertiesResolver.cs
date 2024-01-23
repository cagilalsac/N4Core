#nullable disable

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace N4Core.Resolvers
{
    public class JsonIgnorePropertiesResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);
            if (jsonProperty.PropertyName.Equals("IsDeleted") || jsonProperty.PropertyName.Equals("FileData") || 
                jsonProperty.PropertyName.Equals("FileContent") || jsonProperty.PropertyName.Equals("FilePath"))
            {
                jsonProperty.ShouldSerialize = _ => false;
            }
            return jsonProperty;
        }
    }
}
