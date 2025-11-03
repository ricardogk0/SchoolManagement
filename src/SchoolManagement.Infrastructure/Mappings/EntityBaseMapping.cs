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
               .IsRequired()
               .HasColumnName("created_at");

        builder.Property(e => e.CreatedBy)
               .IsRequired()
               .HasColumnName("created_by");

        builder.Property(e => e.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("updated_at");

        builder.Property(e => e.UpdatedBy)
                .IsRequired(false)
                .HasColumnName("updated_by");

        builder.Property(e => e.IsDeleted)
                .IsRequired()
                .HasColumnName("is_deleted");
    }
}
