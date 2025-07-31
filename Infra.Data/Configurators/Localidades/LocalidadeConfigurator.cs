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


            
            builder.Property(l => l.Latitude)
                .IsRequired()
                .HasColumnName($"{prefixo}_LATITUDE"); 
            
            builder.Property(l => l.Longitude)
                .IsRequired()
                .HasColumnName($"{prefixo}_LONGITUDE");

            builder.Property(l => l.Ativo)
                .IsRequired()
                .HasColumnName($"{prefixo}_ATIVO");

            // Ignorar o Value Object Endereco e propriedades de conveniência
            builder.Ignore(l => l.Endereco);
            builder.Ignore(l => l.Estado);
            builder.Ignore(l => l.Cidade);
            builder.Ignore(l => l.Cep);
            builder.Ignore(l => l.Bairro);
            builder.Ignore(l => l.Logradouro);
            builder.Ignore(l => l.Numero);
            builder.Ignore(l => l.Complemento);
            builder.Ignore(l => l.Uf);
            builder.Ignore(l => l.EnderecoCompleto);
        }
    }
}
