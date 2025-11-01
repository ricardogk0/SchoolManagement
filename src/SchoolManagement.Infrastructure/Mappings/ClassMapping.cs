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
               .HasMaxLength(100);

        builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(255);
    }
}
