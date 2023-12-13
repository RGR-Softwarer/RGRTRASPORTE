using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using Infra.Data.Context;
using Infra.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Infra.Ioc
{
    public static class ContainerServicesCollections
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Painel de indicadores", Version = "v1" });
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


            return services;
        }

    }
}
