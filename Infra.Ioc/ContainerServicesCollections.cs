using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service;
using Infra.CrossCutting.Handlers.Notifications;
using Infra.Data.Context;
using Infra.Data.Data;
using Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Service;

namespace Infra.Ioc
{
    public static class ContainerServicesCollections
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RGR Trasporte", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RGRContext>(options => options.UseNpgsql(configuration.GetValue<string>("ConnectionStrings:RGRTRASPORTE")));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INotificationHandler, NotificationHandler>();

            services.AddScoped<IVeiculoService, VeiculoService>();

            return services;
        }

        public static IServiceCollection AddRepositorys(this IServiceCollection services)
        {
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();

            return services;
        }

    }
}
