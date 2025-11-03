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
               .HasColumnName("name")
               .HasMaxLength(100);

        builder.Property(s => s.BirthDate)
               .IsRequired()
               .HasColumnName("birth_date");

        builder.Property(s => s.DocumentNumber)
               .IsRequired()
               .HasColumnName("document_number")
               .HasMaxLength(20);

        builder.Property(s => s.Email)
               .IsRequired()
               .HasColumnName("email")
               .HasMaxLength(100);

        builder.Property(s => s.Password)
               .IsRequired()
               .HasColumnName("password")
               .HasMaxLength(255);
    }
}
