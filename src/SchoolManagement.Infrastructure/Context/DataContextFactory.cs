using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace SchoolManagement.Infrastructure.Context;

public class DataContextFactory : IDesignTimeDbContextFactory<DbContext>
{
    public DbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        var apiProjectPath = Path.Combine(basePath, "SchoolManagement.API");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetSection("DatabaseSettings:DefaultConnection").Value;

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("The connection string 'DefaultConnection' was not found.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new DataContext(optionsBuilder.Options);
    }
}
