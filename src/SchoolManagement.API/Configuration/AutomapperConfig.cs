using AutoMapper;
using System.Reflection;

namespace SchoolManagement.API.Configuration;

public static class AutomapperConfig
{
    public static void ConfigureAutomapper(this IServiceCollection services)
    {
        var mappingProfiles = GetMappingProfiles();
        if (mappingProfiles.Any())
        {
            services.AddAutoMapper(cfg =>
            {
                foreach (var profileType in mappingProfiles)
                {
                    cfg.AddProfile(profileType);
                }
            });
        }
    }

    private static Type[] GetMappingProfiles()
    {
        var assembly = typeof(SchoolManagement.Service.Mappers.StudentMapper).Assembly;
        return assembly.GetTypes()
            .Where(type => typeof(Profile).IsAssignableFrom(type) && !type.IsAbstract && type.Namespace == "SchoolManagement.Service.Mappers")
            .ToArray();
    }
}
