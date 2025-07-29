using Dominio.Entidades.Auditoria;
using Dominio.Entidades;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Auditoria
{
    internal class RegistroAuditoriaConfigurator : BaseEntityConfigurator<RegistroAuditoria>
    {
        protected override void InternalConfigure(EntityTypeBuilder<RegistroAuditoria> builder)
        {
            prefixo = "RAU";

            builder.ToTable("T_REGISTRO_AUDITORIA");

            builder.Property(e => e.NomeEntidade)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName($"{prefixo}_NOME_ENTIDADE");

            builder.Property(e => e.EntidadeId)
                .IsRequired()
                .HasColumnName($"{prefixo}_ENTIDADE_ID");

            builder.Property(e => e.Acao)
                .IsRequired()
                .HasColumnName($"{prefixo}_ACAO");

            builder.Property(e => e.Dados)
                .HasColumnType("TEXT")
                .HasColumnName($"{prefixo}_DADOS");

            builder.Property(e => e.Usuario)
                .HasMaxLength(100)
                .HasColumnName($"{prefixo}_USUARIO");

            builder.Property(e => e.IP)
                .HasMaxLength(50)
                .HasColumnName($"{prefixo}_IP");

            builder.Property(e => e.DataOcorrencia)
                .IsRequired()
                .HasColumnName($"{prefixo}_DATA_OCORRENCIA");
        }
    }
} 