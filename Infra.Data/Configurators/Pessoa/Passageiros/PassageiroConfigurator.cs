using Dominio.Entidades.Pessoas.Passageiros;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Pessoa.Passageiros
{
    internal class PassageiroConfigurator : BaseEntityConfigurator<Passageiro>
    {
        protected override void InternalConfigure(EntityTypeBuilder<Passageiro> builder)
        {
            prefixo = "PAS";

            builder.ToTable("T_PASSAGEIRO");

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasColumnName($"{prefixo}_NOME");

            builder.Property(p => p.CPF)
                .IsRequired()
                .HasColumnName($"{prefixo}_CPF");

            builder.Property(p => p.Telefone)
                .IsRequired()
                .HasColumnName($"{prefixo}_TELEFONE");

            builder.Property(p => p.Email)
                .IsRequired()
                .HasColumnName($"{prefixo}_EMAIL");
            
            builder.Property(p => p.Observacao)
                .IsRequired()
                .HasColumnName($"{prefixo}_OBSERVACAO");

            builder.Property(p => p.Sexo)
                .IsRequired()
                .HasColumnName($"{prefixo}_SEXO");

            builder.Property(p => p.Situacao)
                .IsRequired()
                .HasColumnName($"{prefixo}_SITUACAO");

            builder.Property(p => p.LocalidadeId)
                .IsRequired()
                .HasColumnName($"LOC_ID");

            builder.HasOne(p => p.Localidade)
                .WithMany()
                .HasForeignKey(p => p.LocalidadeId);

            builder.Property(p => p.LocalidadeDesembarqueId)
                .IsRequired()
                .HasColumnName($"LOC_DESEMBARQUE_ID");

            builder.HasOne(p => p.LocalidadeDesembarque)
                .WithMany()
                .HasForeignKey(p => p.LocalidadeDesembarqueId);

            builder.Property(p => p.LocalidadeEmbarqueId)
                .IsRequired()
                .HasColumnName($"LOC_EMBARQUE_ID");

            builder.HasOne(p => p.LocalidadeEmbarque)
                .WithMany()
                .HasForeignKey(p => p.LocalidadeEmbarqueId);
        }
    }
}
