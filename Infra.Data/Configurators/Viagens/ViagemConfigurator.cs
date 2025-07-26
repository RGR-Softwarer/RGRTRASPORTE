using Dominio.Entidades.Viagens;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.Viagens
{
    internal class ViagemConfigurator : BaseEntityConfigurator<Viagem>
    {
        protected override void InternalConfigure(EntityTypeBuilder<Viagem> builder)
        {
            prefixo = "VIA";

            builder.ToTable("T_VIAGEM");

            builder.Property(v => v.Codigo)
                .IsRequired()
                .HasColumnName($"{prefixo}_CODIGO");

            builder.Property(v => v.DataViagem)
                .IsRequired()
                .HasColumnName($"{prefixo}_DATA");

            builder.Property(v => v.HorarioSaida)
                .IsRequired()
                .HasColumnName($"{prefixo}_HORA_SAIDA");

            builder.Property(v => v.HorarioChegada)
                .IsRequired()
                .HasColumnName($"{prefixo}_HORA_CHEGADA");

            builder.Property(v => v.VeiculoId)
                .IsRequired()
                .HasColumnName($"VEI_ID");

            builder.HasOne(v => v.Veiculo)
                .WithMany()
                .HasForeignKey(v => v.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(v => v.MotoristaId)
                .IsRequired()
                .HasColumnName($"MOT_ID");

            builder.HasOne(v => v.Motorista)
                .WithMany()
                .HasForeignKey(v => v.MotoristaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(v => v.LocalidadeOrigemId)
                .IsRequired()
                .HasColumnName($"LOC_ORIGEM_ID");

            builder.HasOne(v => v.LocalidadeOrigem)
                .WithMany()
                .HasForeignKey(v => v.LocalidadeOrigemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(v => v.LocalidadeDestinoId)
                .IsRequired()
                .HasColumnName($"LOC_DESTINO_ID");

            builder.HasOne(v => v.LocalidadeDestino)
                .WithMany()
                .HasForeignKey(v => v.LocalidadeDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(v => v.GatilhoViagemId)
                .HasColumnName($"GAT_ID");

            builder.HasOne(v => v.GatilhoViagem)
                .WithMany()
                .HasForeignKey(v => v.GatilhoViagemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(v => v.ValorPassagem)
                .IsRequired()
                .HasColumnName($"{prefixo}_VALOR_PASSAGEM");

            builder.Property(v => v.QuantidadeVagas)
                .IsRequired()
                .HasColumnName($"{prefixo}_QUANTIDADE_VAGAS");

            builder.Property(v => v.VagasDisponiveis)
                .IsRequired()
                .HasColumnName($"{prefixo}_VAGAS_DISPONIVEIS");

            builder.Property(v => v.Distancia)
                .IsRequired()
                .HasColumnName($"{prefixo}_DISTANCIA");

            builder.Property(v => v.DescricaoViagem)
                .IsRequired()
                .HasColumnName($"{prefixo}_DESCRICAO");

            builder.Property(v => v.PolilinhaRota)
                .IsRequired()
                .HasColumnName($"{prefixo}_POLILINHA_ROTA");

            builder.Property(v => v.Ativo)
                .IsRequired()
                .HasColumnName($"{prefixo}_ATIVO");

            builder.Property(v => v.Situacao)
                .IsRequired()
                .HasColumnName($"{prefixo}_SITUACAO");

            builder.Property(v => v.Lotado)
                .IsRequired()
                .HasColumnName($"{prefixo}_LOTADO");

            builder.Property(v => v.DataInicioViagem)
                .HasColumnName($"{prefixo}_DATA_INICIO");

            builder.Property(v => v.DataFimViagem)
                .HasColumnName($"{prefixo}_DATA_FIM");

            builder.HasMany(v => v.Passageiros)
                .WithOne(p => p.Viagem)
                .HasForeignKey(p => p.ViagemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(v => v.Posicoes)
                .WithOne(p => p.Viagem)
                .HasForeignKey(p => p.ViagemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
