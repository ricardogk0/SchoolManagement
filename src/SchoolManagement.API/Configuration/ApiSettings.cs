using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Entities.Validations;
using SchoolManagement.Domain.Interfaces.Notifications;
using SchoolManagement.Domain.Interfaces.Repositories;
using SchoolManagement.Domain.Interfaces.Services;
using SchoolManagement.Domain.Interfaces.UoW;
using SchoolManagement.Domain.Notifications;
using SchoolManagement.Infrastructure.Context;
using SchoolManagement.Infrastructure.Repositories;
using SchoolManagement.Infrastructure.UoW;
using SchoolManagement.Service.Resources;
using SchoolManagement.Service.Services;
using SchoolManagement.API.Security;
using System.Text;
using System.Text.Json.Serialization;

namespace SchoolManagement.API.Configuration;

public static class ApiSettings
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        configuration = BuildConfiguration(environment);
        services.AddSingleton(configuration);

        services.AddSingleton(ValidationErrorMessages(configuration));
        ConfigureDatabase(services, configuration);
        ConfigureControllers(services);
        RegisterServices(services, configuration);
        RegisterRepositories(services);
        RegisterValidators(services, configuration);
        RegisterAdditionalServices(services);
        ConfigureAuthenticationAndAuthorization(services, configuration);
    }

    private static IConfiguration BuildConfiguration(IWebHostEnvironment environment)
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("DatabaseSettings:DefaultConnection").Value;
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(connectionString));
    }

    private static void ConfigureControllers(IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
    }
    private static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IClassService, ClassService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
    }

    private static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IClassRepository, ClassRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
    }
    private static void RegisterValidators(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IValidator<StudentEntity>, StudentValidator>();
        services.AddScoped<IValidator<ClassEntity>, ClassValidator>();
    }

    private static void RegisterAdditionalServices(this IServiceCollection services)
    {
        services.ConfigureAutomapper();
        services.TryAddScoped<IUnitOfWork, UnitOfWork>();
        services.TryAddScoped<INotifier, Notifier>();
        services.AddSwaggerConfig();
        services.AddApiVersioningConfiguration();
        services.AddHttpContextAccessor();
    }

    private static void ConfigureAuthenticationAndAuthorization(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        var jwtSection = configuration.GetSection("Jwt");
        var secret = jwtSection.GetValue<string>("Secret") ?? "change_this_dev_secret_please";
        var issuer = jwtSection.GetValue<string>("Issuer") ?? "SchoolManagement";
        var audience = jwtSection.GetValue<string>("Audience") ?? "SchoolManagementAudience";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("Admin")
                .Build();
        });

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
    }

    private static ValidationErrorMessages ValidationErrorMessages(IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(ValidationErrorMessages));

        var errorMessages = section.Get<ValidationErrorMessages>();

        return errorMessages ?? new ValidationErrorMessages();
    }
}