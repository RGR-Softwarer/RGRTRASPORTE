using Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.ValueObjects
{
    public static class ChassiConfigurator
    {
        public static void ConfigureChassi<T>(this PropertyBuilder<Chassi> builder, string columnName = "CHASSI")
            where T : class
        {
            builder.HasConversion(
                chassi => chassi.Numero,
                numero => new Chassi(numero))
                .IsRequired()
                .HasMaxLength(17)
                .HasColumnName(columnName);
        }
    }
}
