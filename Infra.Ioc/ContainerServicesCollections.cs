﻿using Application.Commands.ViagemPassageiro;
using Dominio.Interfaces.Hangfire;
using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using Infra.CrossCutting.Multitenancy;
using Infra.Data.Context;
using Infra.Data.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
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

        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddHttpContextAccessor();
            services.AddScoped<ITenantProvider, TenantProvider>();

            services.AddDbContext<TransportadorContext>((provider, options) =>
            {
                var tenantProvider = provider.GetRequiredService<ITenantProvider>();
                options.UseNpgsql(tenantProvider.GetTenantConnectionString());
            });

            services.AddDbContext<CadastroContext>((provider, options) =>
            {
                var tenantProvider = provider.GetRequiredService<ITenantProvider>();
                options.UseNpgsql(tenantProvider.GetTenantConnectionString());
            });

            services.AddScoped<IUnitOfWorkContext>(provider => provider.GetRequiredService<TransportadorContext>());
            services.AddScoped<IUnitOfWorkContext>(provider => provider.GetRequiredService<CadastroContext>());

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INotificationHandler, NotificationHandler>();

            var serviceAssembly = Assembly.GetAssembly(typeof(Service.Services.Localidades.LocalidadeService));

            var serviceTypes = serviceAssembly.GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("Service") && !t.IsAbstract);

            foreach (var implementationType in serviceTypes)
            {
                var interfaceType = implementationType.GetInterfaces()
                    .FirstOrDefault(i => i.Name.EndsWith(implementationType.Name));

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementationType);
                }
            }

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AdicionarViagemCommand).Assembly));
            // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProcessarViagemCriadaCommandHandler).Assembly));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Application.Behaviors.UnitOfWorkBehavior<,>));

            return services;
        }

        public static IServiceCollection AddRepositorys(this IServiceCollection services)
        {
            var repositoryAssembly = Assembly.GetAssembly(typeof(Infra.Data.Repositories.Localidades.LocalidadeRepository));

            var repositoryTypes = repositoryAssembly.GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("Repository") && !t.IsAbstract);

            foreach (var implementationType in repositoryTypes)
            {
                var interfaceType = implementationType.GetInterfaces()
                    .FirstOrDefault(i => i.Name.EndsWith(implementationType.Name));

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementationType);
                }
            }

            return services;
        }

    }
}
