using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SchoolManagement.Infrastructure.Mappings;

public abstract class EntityBaseMapping<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.Property(e => e.CreateBy)
               .IsRequired();

        builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

        builder.Property(e => e.UpdateBy)
                .IsRequired(false);

        builder.Property(e => e.IsDeleted)
                .IsRequired();
    }
}
