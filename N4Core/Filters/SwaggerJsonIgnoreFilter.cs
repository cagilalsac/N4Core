using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace N4Core.Filters
{
    public class SwaggerJsonIgnoreFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters().SelectMany(p => p.ParameterType.GetProperties()
                .Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() != null || p.Name.EndsWith("Output") || 
                p.Name.Equals("Guid") || p.Name.Equals("IsDeleted") || p.Name.Equals("FileData") || p.Name.Equals("FileContent") || p.Name.Equals("FilePath")));
            if (ignoredProperties.Any())
            {
                foreach (var ignoredProperty in ignoredProperties)
                {
                    operation.Parameters = operation.Parameters.Where(p => !p.Name.Equals(ignoredProperty.Name)).ToList();
                }
            }
        }
    }
}
