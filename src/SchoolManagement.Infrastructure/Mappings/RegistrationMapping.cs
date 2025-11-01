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

        builder.HasOne(r => r.Student)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.HasOne(r => r.Class)
               .WithMany()
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();
    }
}
