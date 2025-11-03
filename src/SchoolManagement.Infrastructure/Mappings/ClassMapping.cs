using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Infrastructure.Mappings;

public class ClassMapping : EntityBaseMapping<ClassEntity>
{
    public override void Configure(EntityTypeBuilder<ClassEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("classes");

        builder.Property(c => c.ClassName)
               .IsRequired()
               .HasColumnName("class_name")
               .HasMaxLength(100);

        builder.Property(c => c.Description)
                .IsRequired()
                .HasColumnName("description")
                .HasMaxLength(255);
    }
}
