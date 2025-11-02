using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace SchoolManagement.API.Configuration;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            ConfigureSwaggerOperations(c);
            ConfigureSwaggerSecurity(c);
            c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        return services;
    }

    private static void ConfigureSwaggerOperations(SwaggerGenOptions options)
    {
        options.OperationFilter<SwaggerDefaultValues>();
        options.OperationFilter<FileUploadOperation>();
    }

    private static void ConfigureSwaggerSecurity(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Insert the JWT token this way: Bearer {your token}",
            Name = "Authorization",
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    }

    public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });

        return app;
    }
}

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var versionText = description.GroupName.StartsWith("v", StringComparison.OrdinalIgnoreCase)
            ? description.GroupName.Substring(1)
            : description.ApiVersion.MajorVersion.ToString();

        var info = new OpenApiInfo
        {
            Title = "School Management API",
            Version = versionText,
            Description = "School Management.",
            Contact = new OpenApiContact { Name = "Ricardo Gomes", Email = "ricardokevi@gmail.com" },
            License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This version is obsolete!";
        }

        return info;
    }
}

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();

        foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
        {
            var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
            if (operation.Responses.TryGetValue(responseKey, out var response))
            {
                foreach (var contentType in response.Content.Keys)
                {
                    if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                    {
                        response.Content.Remove(contentType);
                    }
                }
            }
        }

        if (operation.Parameters == null) return;

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.FirstOrDefault(p => p.Name == parameter.Name);
            if (description == null) continue;

            var modelMetadata = description.ModelMetadata;
            parameter.Description ??= modelMetadata?.Description;

            if (parameter.Schema.Default == null && description.DefaultValue != null)
            {
                var targetType = modelMetadata?.ModelType ?? description.Type;
                var json = JsonSerializer.Serialize(description.DefaultValue, targetType);
                parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
            }

            parameter.Required |= description.IsRequired;
        }
    }
}

public class FileUploadOperation : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParameters = context.ApiDescription.ParameterDescriptions
            .Where(p => p.Type == typeof(IFormFile) || p.Type == typeof(IEnumerable<IFormFile>))
            .ToList();

        foreach (var fileParameter in fileParameters)
        {
            var mediaType = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                }
            };

            operation.RequestBody = new OpenApiRequestBody
            {
                Content = { ["multipart/form-data"] = mediaType }
            };
        }
    }
}

public class ReplaceVersionWithExactValueInPath : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var updatedPaths = new OpenApiPaths();
        foreach (var kvp in swaggerDoc.Paths)
        {
            var original = kvp.Key;
            var replaced = original
                .Replace("v{version}", $"v{swaggerDoc.Info.Version}")
                .Replace("v{version:apiVersion}", $"v{swaggerDoc.Info.Version}");

            updatedPaths.Add(replaced, kvp.Value);
        }

        swaggerDoc.Paths = updatedPaths;
    }
}
