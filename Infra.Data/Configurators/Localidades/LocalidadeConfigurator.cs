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
            prefixo = "LOC";

            builder.ToTable("T_LOCALIDADE");

            builder.Property(l => l.Nome)
                .IsRequired()
                .HasColumnName($"{prefixo}_NOME");

            builder.Property(l => l.Cep)
                .IsRequired()
                .HasColumnName($"{prefixo}_CEP");

            builder.Property(l => l.Uf)
                .IsRequired()
                .HasColumnName($"{prefixo}_UF");

            builder.Property(l => l.Cidade)
                .IsRequired()
                .HasColumnName($"{prefixo}_CIDADE");

            builder.Property(l => l.Bairro)
                .IsRequired()
                .HasColumnName($"{prefixo}_BAIRRO");

            builder.Property(l => l.Logradouro)
                .IsRequired()
                .HasColumnName($"{prefixo}_LOGRADOURO");

            builder.Property(l => l.Complemento)
                .IsRequired()
                .HasColumnName($"{prefixo}_COMPLEMENTO"); 
            
            builder.Property(l => l.Numero)
                .IsRequired()
                .HasColumnName($"{prefixo}_NUMERO"); 
            
            builder.Property(l => l.Latitude)
                .IsRequired()
                .HasColumnName($"{prefixo}_LATITUDE"); 
            
            builder.Property(l => l.Longitude)
                .IsRequired()
                .HasColumnName($"{prefixo}_LONGITUDE");
        }
    }
}
