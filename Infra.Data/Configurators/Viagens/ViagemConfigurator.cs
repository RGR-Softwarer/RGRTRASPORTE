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

            builder.Property(v => v.CodigoViagem)
                .IsRequired()
                .HasColumnName($"{prefixo}_CODIGO_VIAGEM");

            builder.Property(v => v.DataViagem)
                .IsRequired()
                .HasColumnName($"{prefixo}_DATA_VIAGEM");

            builder.Property(v => v.VeiculoId)
                .IsRequired()
                .HasColumnName($"VEI_ID");

            builder.Property(v => v.MotoristaId)
                .IsRequired()
                .HasColumnName($"MOT_ID");
            
            builder.Property(v => v.GatilhoViagemId)
                .IsRequired()
                .HasColumnName($"GAV_ID");

            builder.Property(v => v.OrigemId)
                .IsRequired()
                .HasColumnName($"LOC_ORIGEM_ID");

            builder.Property(v => v.DestinoId)
                .IsRequired()
                .HasColumnName($"LOC_DESTINO_ID");

            builder.Property(v => v.HorarioSaida)
                .IsRequired()
                .HasColumnName($"{prefixo}_HORARIO_SAIDA");

            builder.Property(v => v.HorarioChegada)
                .IsRequired()
                .HasColumnName($"{prefixo}_HORARIO_CHEGADA");

            builder.Property(v => v.Situacao)
                .IsRequired()
                .HasColumnName($"{prefixo}_SITUACAO");

            builder.Property(v => v.Distancia)
                .IsRequired()
                .HasColumnName($"{prefixo}_DISTANCIA");

            builder.Property(v => v.NumeroPassageiros)
                .IsRequired()
                .HasColumnName($"{prefixo}_NUMERO_PASSAGEIROS");

            builder.Property(v => v.Lotado)
                .IsRequired()
                .HasColumnName($"{prefixo}_LOTADO");

            builder.Property(v => v.Excesso)
                .IsRequired()
                .HasColumnName($"{prefixo}_EXCESSO");
            
            builder.Property(v => v.MotivoProblema)
                .IsRequired()
                .HasColumnName($"{prefixo}_MOTIVO_PROBLEMA"); 
            
            builder.Property(v => v.DescricaoViagem)
                .IsRequired()
                .HasColumnName($"{prefixo}_DESCRICAO_VIAGEM");
            
            builder.Property(v => v.LatitudeFimViagem)
                .IsRequired()
                .HasColumnName($"{prefixo}_LATITUDE_FIM_VIAGEM");
            
            builder.Property(v => v.LatitudeInicioViagem)
                .IsRequired()
                .HasColumnName($"{prefixo}_LATITUDE_INICIO_VIAGEM");
            
            builder.Property(v => v.DistanciaRealizada)
                .IsRequired()
                .HasColumnName($"{prefixo}_DISTANCIA_REALIZADA");
            
            builder.Property(v => v.PolilinhaRota)
                .IsRequired()
                .HasColumnName($"{prefixo}_POLILINHA_ROTA");
            
            builder.Property(v => v.PolilinhaRotaRealizada)
                .IsRequired()
                .HasColumnName($"{prefixo}_POLILINHA_ROTA_REALIZADA");

            builder.HasOne(v => v.Veiculo)
                .WithMany()
                .HasForeignKey(v => v.VeiculoId);

            //builder.HasOne(v => v.Motorista)
            //    .WithMany()
            //    .HasForeignKey(v => v.MotoristaId);

            //builder.HasOne(v => v.Origem)
            //    .WithMany()
            //    .HasForeignKey(v => v.OrigemId);

            //builder.HasOne(v => v.Destino)
            //    .WithMany()
            //    .HasForeignKey(v => v.DestinoId);
        }
    }
}
