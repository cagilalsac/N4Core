using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace N4Core.Filters.Swagger
{
    public class SwaggerRemoveSchemasFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (KeyValuePair<string, OpenApiSchema> schema in swaggerDoc.Components.Schemas)
            {
                swaggerDoc.Components.Schemas.Remove(schema.Key);
            }
        }
    }
}
