using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Enums.Data;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infra.Data.Configurators.Viagens.Gatilho
{
    internal class GatilhoViagemConfigurator : BaseEntityConfigurator<GatilhoViagem>
    {
        protected override void InternalConfigure(EntityTypeBuilder<GatilhoViagem> builder)
        {
            prefixo = "GAV";

            builder.ToTable("T_GATILHO_VIAGEM");

            builder.Property(g => g.Descricao)
                .IsRequired()
                .HasColumnName($"{prefixo}_DESCRICAO");

            builder.Property(g => g.VeiculoId)
                .IsRequired()
                .HasColumnName($"VEI_ID");

            builder.Property(g => g.MotoristaId)
                .IsRequired()
                .HasColumnName($"MOT_MOTORISTA_ID");

            builder.Property(g => g.OrigemId)
                .IsRequired()
                .HasColumnName($"LOC_ORIGEM_ID");

            builder.Property(g => g.DestinoId)
                .IsRequired()
                .HasColumnName($"LOC_DESTINO_ID");

            builder.Property(g => g.HorarioSaida)
                .IsRequired()
                .HasColumnName($"{prefixo}_HORARIO_SAIDA");

            builder.Property(g => g.HorarioChegada)
                .IsRequired()
                .HasColumnName($"{prefixo}_HORARIO_CHEGADA");

            builder.Property(g => g.Ativo)
                .IsRequired()
                .HasColumnName($"{prefixo}_ATIVO");

            builder.Property(g => g.Distancia)
                .IsRequired()
                .HasColumnName($"{prefixo}_DISTANCIA");

            builder.Property(g => g.DescricaoViagem)
                .HasColumnName($"{prefixo}_DESCRICAO_VIAGEM");

            builder.Property(g => g.PolilinhaRota)
                .HasColumnName($"{prefixo}_POLILHINA_ROTA");

            PropertyBuilder<List<DiaSemanaEnum>> diasSemanaProperty = builder.Property(g => g.DiasSemana);

            diasSemanaProperty.HasConversion(
                v => string.Join(',', v.Select(i => ((int)i).ToString())),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(s => (DiaSemanaEnum)int.Parse(s))
                      .ToList()
            );

            diasSemanaProperty.Metadata.SetValueComparer(
                new ValueComparer<List<DiaSemanaEnum>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, (int)v)),
                    c => c.ToList()
                )
            );

            diasSemanaProperty.HasColumnName($"{prefixo}_DIAS_SEMANA");

            builder.HasOne(g => g.Veiculo)
                .WithMany()
                .HasForeignKey(g => g.VeiculoId);

            //builder.HasOne(g => g.Motorista)
            //    .WithMany()
            //    .HasForeignKey(g => g.MotoristaId);

            //builder.HasOne(g => g.Origem)
            //    .WithMany()
            //    .HasForeignKey(g => g.OrigemId);

            //builder.HasOne(g => g.Destino)
            //    .WithMany()
            //    .HasForeignKey(g => g.DestinoId);

            //builder.HasMany(g => g.Viagens)
            //    .WithOne(v => v.GatilhoViagem)
            //    .HasForeignKey(v => v.GatilhoViagemId);
        }
    }
}
