using Job_Portal.ViewModels;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileUploadParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(JobApplicationViewModel))
            .ToList();

        if (!fileUploadParams.Any()) return;

        operation.Parameters = new List<OpenApiParameter>();
        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>
                        {
                            { "CVFile", new OpenApiSchema { Type = "string", Format = "binary" } },
                            { "JobId", new OpenApiSchema { Type = "integer", Format = "int32" } }
                        },
                        Required = new HashSet<string> { "CVFile", "JobId" }
                    }
                }
            }
        };
    }
}
