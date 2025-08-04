using Application.Behaviors;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service.Hangfire;
using FluentValidation;
using Infra.CrossCutting.Auth;
using Infra.CrossCutting.Cache;
using Infra.CrossCutting.Handlers.Notifications;
using Infra.CrossCutting.Multitenancy;
using Infra.Data.Context;
using Infra.Data.Data;
using Infra.Ioc.HealthChecks;
using Infra.Ioc.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Service.Services.Hangifre;
using System.Reflection;

namespace Infra.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Definição dos assemblies principais
            var applicationAssembly = typeof(Application.Commands.Localidade.CriarLocalidadeCommand).Assembly;
            var domainAssembly = typeof(Dominio.Entidades.BaseEntity).Assembly;
            var infrastructureAssembly = typeof(Infra.Data.Context.TransportadorContext).Assembly;

            // === CONFIGURAÇÃO DOS CONTEXTOS ===
            // Configuração do Entity Framework com Multitenancy
            services.AddDbContext<TransportadorContext>((serviceProvider, options) =>
            {
                var tenantProvider = serviceProvider.GetService<ITenantProvider>();
                var connectionString = tenantProvider?.GetTenantConnectionString() ??
                                     configuration.GetConnectionString("RGRTRASPORTE");
                options.UseNpgsql(connectionString);
            });

            services.AddDbContext<CadastroContext>((serviceProvider, options) =>
            {
                var tenantProvider = serviceProvider.GetService<ITenantProvider>();
                var connectionString = tenantProvider?.GetTenantConnectionString() ??
                                     configuration.GetConnectionString("RGRTRASPORTE");
                options.UseNpgsql(connectionString);
            });

            // === MULTITENANCY E INFRAESTRUTURA ===
            services.AddHttpContextAccessor();
            services.AddScoped<ITenantProvider, TenantProvider>();

            // Registra o contexto principal para UnitOfWork
            services.AddScoped<IUnitOfWorkContext>(provider =>
            {
                var tenantProvider = provider.GetRequiredService<ITenantProvider>();
                var tenantId = tenantProvider.GetTenantId();

                // Lógica para determinar qual contexto usar baseado no tenant
                if (tenantId.Contains("transportador") || tenantId.Contains("localhost"))
                    return provider.GetRequiredService<TransportadorContext>();
                else
                    return provider.GetRequiredService<CadastroContext>();
            });

            // Registra UnitOfWork simples ao invés do MultiContext
            services.AddScoped<IUnitOfWork, MultiContextUnitOfWork>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>();

            // === MEDIATOR E BEHAVIORS ===
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(applicationAssembly);
                cfg.RegisterServicesFromAssembly(domainAssembly);
                cfg.RegisterServicesFromAssembly(infrastructureAssembly);
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // FluentValidation - Registro automático de todos os validators
            services.AddValidatorsFromAssembly(applicationAssembly, includeInternalTypes: true);
            services.AddValidatorsFromAssembly(domainAssembly, includeInternalTypes: true);
            services.AddValidatorsFromAssembly(infrastructureAssembly, includeInternalTypes: true);

            // Pipeline Behaviors - Ordem é crucial!
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));

            // === REGISTRO AUTOMÁTICO DE REPOSITORIES ===
            services.AddAllRepositories(infrastructureAssembly, domainAssembly);

            // === REGISTROS MANUAIS ESPECÍFICOS ===
            // Registro do GenericRepository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Manter apenas o registro do PassageiroRepository que tem implementação específica
            services.AddScoped<Dominio.Interfaces.Infra.Data.Passageiros.IPassageiroRepository, Infra.Data.Repositories.Passageiros.PassageiroRepository>();

            // === REGISTROS MANUAIS ESPECÍFICOS DE SERVICES ===
            // Apenas serviços cross-cutting
            services.AddScoped<INotificationContext, NotificationContext>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IHangfireService, HangfireService>();

            // === CACHE DISTRIBUÍDO ===
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "RGRTransporte:";
            });

            // === HEALTHCHECKS ===
            var connectionString = configuration.GetConnectionString("RGRTRASPORTE") ?? string.Empty;
            services.AddCustomHealthChecks(connectionString);

            // === LOGGING ===
            services.AddSerilogLogging(configuration);

            return services;
        }

        /// <summary>
        /// Adiciona registro automático de todos os Repositories por convenção
        /// </summary>
        public static IServiceCollection AddAllRepositories(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                // Busca por classes concretas que terminam com "Repository"
                var repositoryTypes = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface && !t.IsGenericTypeDefinition)
                    .Where(t => t.Name.EndsWith("Repository"))
                    .Where(t => !t.Name.StartsWith("Generic")) // Ignora GenericRepository
                    .Where(t => !t.Name.StartsWith("Base")) // Ignora classes base
                    .ToList();

                foreach (var repositoryType in repositoryTypes)
                {
                    // Encontra interfaces específicas (não genéricas abertas)
                    var interfaces = repositoryType.GetInterfaces()
                        .Where(i => i.Name.EndsWith("Repository"))
                        .Where(i => !i.IsGenericTypeDefinition) // Ignora tipos genéricos abertos
                        .Where(i => !i.Name.StartsWith("IGeneric")) // Ignora IGenericRepository
                        .ToList();

                    foreach (var interfaceType in interfaces)
                    {
                        if (!services.Any(s => s.ServiceType == interfaceType))
                        {
                            services.AddScoped(interfaceType, repositoryType);
                        }
                    }
                }
            }

            return services;
        }

        /// <summary>
        /// Adiciona registro automático de todos os Services por convenção
        /// </summary>
        public static IServiceCollection AddAllServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            // Removido pois não há mais necessidade de registro automático de serviços de domínio
            return services;
        }

        /// <summary>
        /// Adiciona registro automático de todos os Command Handlers no assembly
        /// </summary>
        public static IServiceCollection AddAllCommandHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .Where(t => !t.IsAbstract && !t.IsInterface);

            foreach (var handlerType in handlerTypes)
            {
                var interfaceTypes = handlerType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddScoped(interfaceType, handlerType);
                }
            }

            return services;
        }

        /// <summary>
        /// Adiciona registro automático de todos os Query Handlers no assembly
        /// </summary>
        public static IServiceCollection AddAllQueryHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .Where(t => !t.IsAbstract && !t.IsInterface);

            foreach (var handlerType in handlerTypes)
            {
                var interfaceTypes = handlerType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddScoped(interfaceType, handlerType);
                }
            }

            return services;
        }

        /// <summary>
        /// Adiciona registro automático de todos os Validators no assembly
        /// </summary>
        public static IServiceCollection AddAllValidators(this IServiceCollection services, Assembly assembly)
        {
            var validatorTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)))
                .Where(t => !t.IsAbstract && !t.IsInterface);

            foreach (var validatorType in validatorTypes)
            {
                var interfaceTypes = validatorType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

                foreach (var interfaceType in interfaceTypes)
                {
                    services.AddScoped(interfaceType, validatorType);
                }
            }

            return services;
        }

        public static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TransportadorContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("RGRTRASPORTE")));

            services.AddDbContext<CadastroContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("RGRTRASPORTE")));

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RGR Transporte API",
                    Version = "v1",
                    Description = "API para gestão de transportes RGR com Multitenancy"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }

        public static IServiceCollection ConfigureApi(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            return services;
        }
    }
}
