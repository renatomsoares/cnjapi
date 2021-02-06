using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Application.Filters {
    public class IgnorePropertiesSchemaFilter : ISchemaFilter {
        public void Apply (Schema schema, SchemaFilterContext context) {
            if (schema.Properties == null) return;

            schema.Properties = schema.Properties
                .Where (entry => {
                    return !(entry.Key == "notifications");
                })
                .ToDictionary (entry => entry.Key, entry => entry.Value);
        }
    }

}