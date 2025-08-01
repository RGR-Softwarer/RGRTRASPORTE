using Dominio.Entidades.Pessoas;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Pessoa
{
    internal class MotoristaConfigurator : BaseEntityConfigurator<Motorista>
    {
        protected override void InternalConfigure(EntityTypeBuilder<Motorista> builder)
        {
            prefixo = "MOT";

            builder.ToTable("T_MOTORISTA");

            builder.Property(m => m.Nome)
                .IsRequired()
                .HasColumnName($"{prefixo}_NOME");

            builder.Property(m => m.Situacao)
                .IsRequired()
                .HasColumnName($"{prefixo}_SITUACAO");

            builder.Property(m => m.CPF)
                .HasConversion(
                    cpf => cpf.Numero,
                    numero => new Dominio.ValueObjects.CPF(numero))
                .IsRequired()
                .HasColumnName($"{prefixo}_CPF");

            builder.Property(m => m.RG)
                .HasColumnName($"{prefixo}_RG");

            builder.Property(m => m.Telefone)
                .IsRequired()
                .HasColumnName($"{prefixo}_TELEFONE");

            builder.Property(m => m.Email)
                .IsRequired()
                .HasColumnName($"{prefixo}_EMAIL");

            builder.Property(m => m.CNH)
                .IsRequired()
                .HasColumnName($"{prefixo}_CNH");

            builder.Property(m => m.CategoriaCNH)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName($"{prefixo}_CATEGORIA_CNH");

            builder.Property(m => m.ValidadeCNH)
                .IsRequired()
                .HasColumnName($"{prefixo}_VALIDADE_CNH");

            builder.Property(m => m.Observacao)
                .HasColumnName($"{prefixo}_OBSERVACAO");

            builder.Property(m => m.LocalidadeId)
                .IsRequired()
                .HasColumnName($"LOC_ID");

            builder.Property(m => m.Sexo)
                .IsRequired()
                .HasConversion<int>()
                .HasColumnName($"{prefixo}_SEXO");

            // Ignorar propriedades que n�o fazem sentido para Motorista
            builder.Ignore(m => m.LocalidadeEmbarqueId);
            builder.Ignore(m => m.LocalidadeDesembarqueId);
        }
    }
}
