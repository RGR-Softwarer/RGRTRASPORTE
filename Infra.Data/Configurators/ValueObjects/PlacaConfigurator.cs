using Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.ValueObjects
{
    public static class PlacaConfigurator
    {
        public static void ConfigurePlaca<T>(this PropertyBuilder<Placa> builder, string columnName = "PLACA")
            where T : class
        {
            builder.HasConversion(
                placa => placa.Numero,
                numero => new Placa(numero))
                .IsRequired()
                .HasMaxLength(7)
                .HasColumnName(columnName);
        }
    }
}
