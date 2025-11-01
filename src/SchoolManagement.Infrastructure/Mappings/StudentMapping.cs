using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Infrastructure.Mappings;

public class StudentMapping : EntityBaseMapping<StudentEntity>
{
    public override void Configure(EntityTypeBuilder<StudentEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("students");

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.BirthDate)
               .IsRequired();

        builder.Property(s => s.DocumentNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(s => s.Email)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(s => s.Password)
               .IsRequired()
               .HasMaxLength(255);
    }
}
