using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Mappings;

namespace SchoolManagement.Infrastructure.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<StudentEntity> Students { get; set; }
    public DbSet<ClassEntity> Classes { get; set; }
    public DbSet<RegistrationEntity> Registrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO colocar uma variavel de ambiente para habilitar/desabilitar codefirst
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new StudentMapping());
        modelBuilder.ApplyConfiguration(new ClassMapping());
        modelBuilder.ApplyConfiguration(new RegistrationMapping());
    }
}
