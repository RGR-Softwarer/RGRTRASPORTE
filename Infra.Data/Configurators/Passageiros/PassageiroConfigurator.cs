using Dominio.Entidades.Pessoas.Passageiros;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Passageiros
{
    internal class PassageiroConfigurator : BaseEntityConfigurator<Passageiro>
    {
        protected override void InternalConfigure(EntityTypeBuilder<Passageiro> builder)
        {
            builder.Property(p => p.Nome).IsRequired();
            builder.Property(p => p.CPF).IsRequired();
            builder.Property(p => p.Telefone).IsRequired();
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.Sexo).IsRequired();
            builder.Property(p => p.LocalidadeId).IsRequired();

            builder.HasOne(p => p.Localidade).WithMany().HasForeignKey(p => p.LocalidadeId);

            builder.ToTable(nameof(Passageiro));
        }
    }
}