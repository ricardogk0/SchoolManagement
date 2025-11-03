using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Infrastructure.Mappings;

public class RegistrationMapping : EntityBaseMapping<RegistrationEntity>
{
    public override void Configure(EntityTypeBuilder<RegistrationEntity> builder)
    {
        base.Configure(builder);

        builder.ToTable("registrations");

        builder.Property<Guid>("StudentId")
               .HasColumnName("student_id")
               .IsRequired();

        builder.HasOne(r => r.Student)
               .WithMany()
               .HasForeignKey("StudentId")
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.Property<Guid>("ClassId")
               .HasColumnName("class_id")
               .IsRequired();

        builder.HasOne(r => r.Class)
               .WithMany()
               .HasForeignKey("ClassId")
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();
    }
}
