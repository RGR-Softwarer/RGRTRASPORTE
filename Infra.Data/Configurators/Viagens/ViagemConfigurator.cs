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

            // Mapeamento básico do Codigo (será convertido internamente pela entidade)
            builder.Property(v => v.Codigo)
                .HasConversion(
                    codigo => codigo.Valor,
                    valor => new Dominio.ValueObjects.CodigoViagem(valor))
                .IsRequired()
                .HasColumnName($"{prefixo}_CODIGO");

            // Propriedades do PeriodoViagem mapeadas para shadow properties para manter compatibilidade
            builder.Property<DateTime>("DataViagem")
                .IsRequired()
                .HasColumnName($"{prefixo}_DATA");

            builder.Property<TimeSpan>("HorarioSaida")
                .IsRequired()
                .HasColumnName($"{prefixo}_HORA_SAIDA");

            builder.Property<TimeSpan>("HorarioChegada")
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

            builder.Property(v => v.LocalidadeOrigemId)
                .IsRequired()
                .HasColumnName($"LOC_ORIGEM_ID");

            builder.Property(v => v.LocalidadeDestinoId)
                .IsRequired()
                .HasColumnName($"LOC_DESTINO_ID");

            builder.Property(v => v.GatilhoViagemId)
                .HasColumnName($"GAV_ID");

            builder.HasOne(v => v.GatilhoViagem)
                .WithMany()
                .HasForeignKey(v => v.GatilhoViagemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(v => v.QuantidadeVagas)
                .IsRequired()
                .HasColumnName($"{prefixo}_QUANTIDADE_VAGAS");

            builder.Property(v => v.VagasDisponiveis)
                .IsRequired()
                .HasColumnName($"{prefixo}_VAGAS_DISPONIVEIS");

            // Mapeamento do Value Object Distancia
            builder.Property(v => v.Distancia)
                .HasConversion(
                    distancia => distancia.Quilometros,
                    km => new Dominio.ValueObjects.Distancia(km))
                .IsRequired()
                .HasColumnName($"{prefixo}_DISTANCIA");

            builder.Property(v => v.DescricaoViagem)
                .IsRequired()
                .HasColumnName($"{prefixo}_DESCRICAO");

            // Mapeamento do Value Object Polilinha
            builder.Property(v => v.PolilinhaRota)
                .HasConversion(
                    polilinha => polilinha.Rota,
                    rota => new Dominio.ValueObjects.Polilinha(rota))
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

            // Ignorar o Value Object Periodo para mapear propriedades individuais
            builder.Ignore(v => v.Periodo);

            // Relacionamento com Veiculo (agora no mesmo contexto)
            builder.HasOne(v => v.Veiculo)
                .WithMany()
                .HasForeignKey(v => v.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ignorar propriedades de navegação para entidades do CadastroContext
            builder.Ignore(v => v.Motorista);
            builder.Ignore(v => v.LocalidadeOrigem);
            builder.Ignore(v => v.LocalidadeDestino);
        }
    }
}
