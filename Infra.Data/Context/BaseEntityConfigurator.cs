using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Globalization;

namespace Infra.Data.Context
{
    public abstract class BaseEntityConfigurator<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        protected string prefixo = string.Empty;

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            InternalConfigure(builder);

            builder.HasKey(x => x.Id);

            builder.Property(p => p.Id)
                .IsRequired()
                .HasColumnName($"{prefixo}_ID");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName($"{prefixo}_CREATED_AT");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnName($"{prefixo}_UPDATED_AT");

        }

        protected abstract void InternalConfigure(EntityTypeBuilder<TEntity> builder);
    }
}
