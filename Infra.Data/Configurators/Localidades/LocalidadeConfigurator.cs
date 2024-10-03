using Dominio.Entidades.Localidades;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Localidades
{
    internal class LocalidadeConfigurator : BaseEntityConfigurator<Localidade>
    {
        protected override void InternalConfigure(EntityTypeBuilder<Localidade> builder)
        {
            builder.Property(l => l.Nome).IsRequired();
            builder.Property(l => l.Cep).IsRequired();
            builder.Property(l => l.Uf).IsRequired();
            builder.Property(l => l.Cidade).IsRequired();
            builder.Property(l => l.Bairro).IsRequired();
            builder.Property(l => l.Logradouro).IsRequired();

            builder.ToTable(nameof(Localidade));
        }
    }
}