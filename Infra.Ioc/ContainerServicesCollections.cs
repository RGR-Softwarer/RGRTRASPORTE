using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service;
using Infra.CrossCutting.Handlers.Notifications;
using Infra.Data.Context;
using Infra.Data.Data;
using Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Service.Services;
using System.Text.Json.Serialization;

namespace Infra.Ioc
{
    public static class ContainerServicesCollections
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RGR Trasporte", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddContext(this IServiceCollection services, string queryString)
        {
            services.AddDbContext<RGRContext>(options => options.UseNpgsql(queryString));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INotificationHandler, NotificationHandler>();

            services.AddScoped<IVeiculoService, VeiculoService>();
            services.AddScoped<IModeloVeicularService, ModeloVeicularService>();

            return services;
        }

        public static IServiceCollection AddRepositorys(this IServiceCollection services)
        {
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();
            services.AddScoped<IModeloVeicularRepository, ModeloVeicularRepository>();

            return services;
        }

    }
}
