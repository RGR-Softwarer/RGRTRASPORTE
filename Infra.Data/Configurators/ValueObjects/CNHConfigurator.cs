using Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Data.Configurators.ValueObjects
{
    public static class CNHConfigurator
    {
        public static void ConfigureCNH<T>(this PropertyBuilder<CNH> builder, string columnPrefix = "CNH")
            where T : class
        {
            builder.HasConversion(
                cnh => cnh.Numero,
                numero => new CNH(numero, Dominio.Enums.Veiculo.CategoriaCNHEnum.B, DateTime.Today.AddYears(5)))
                .IsRequired()
                .HasMaxLength(11);
        }

        public static void ConfigureCNHCompleta<T>(this EntityTypeBuilder<T> builder, 
            string numeroColumn, string categoriaColumn, string validadeColumn)
            where T : class
        {
            // Para casos onde precisamos mapear CNH como campos separados
            // Esta configuração será usada em uma migração futura quando 
            // consolidarmos os campos CNH em um Value Object
        }
    }
}
