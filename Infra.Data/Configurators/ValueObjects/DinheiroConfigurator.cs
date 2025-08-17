using Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.ValueObjects
{
    public static class DinheiroConfigurator
    {
        public static void ConfigureDinheiro<T>(this PropertyBuilder<Dinheiro> builder, string columnName = "VALOR")
            where T : class
        {
            builder.HasConversion(
                dinheiro => dinheiro.Valor,
                valor => Dinheiro.Real(valor))
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnName(columnName);
        }

        public static void ConfigureDinheiroComMoeda<T>(this EntityTypeBuilder<T> builder,
            string valorColumn, string moedaColumn = "MOEDA")
            where T : class
        {
            // Para casos onde queremos armazenar valor e moeda separadamente
            // Esta configuração será usada quando implementarmos suporte completo a múltiplas moedas
        }
    }
}
