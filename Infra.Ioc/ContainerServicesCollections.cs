using Application.Behaviors;
using Dominio.Interfaces.Infra.Data;
using FluentValidation;
using Hangfire;
using Infra.CrossCutting.Handlers.ExceptionHandler;
using Infra.CrossCutting.Handlers.Notifications;
using Infra.CrossCutting.Multitenancy;
using Infra.Data.Context;
using Infra.Data.Data;
using Infra.Ioc.HealthChecks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            services.AddControllers()
                .AddJsonOptions(options =>
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
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "RGR Trasporte", 
                    Version = "v1",
                    Description = "API do sistema RGR Transporte",
                    Contact = new OpenApiContact
                    {
                        Name = "Suporte",
                        Email = "suporte@rgrtransporte.com.br"
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Inclui os comentários XML da API
                var xmlFiles = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .Select(a => new { Assembly = a, Name = $"{a.GetName().Name}.xml" })
                    .Where(x => File.Exists(Path.Combine(AppContext.BaseDirectory, x.Name)))
                    .ToList();

                foreach (var xmlFile in xmlFiles)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile.Name);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                }
            });

            return services;
        }

        public static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ITenantProvider, TenantProvider>();

            var connectionString = configuration.GetConnectionString("RGRTRASPORTE") ?? string.Empty;

            // Configuração dos contextos
            services.AddDbContext<TransportadorContext>((provider, options) =>
            {
                var tenantProvider = provider.GetRequiredService<ITenantProvider>();
                options.UseNpgsql(tenantProvider.GetTenantConnectionString())
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            services.AddDbContext<CadastroContext>((provider, options) =>
            {
                var tenantProvider = provider.GetRequiredService<ITenantProvider>();
                options.UseNpgsql(tenantProvider.GetTenantConnectionString())
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });

            // Registrando os contextos
            services.AddScoped<IUnitOfWorkContext>(provider => provider.GetRequiredService<TransportadorContext>());
            services.AddScoped<IUnitOfWorkContext>(provider => provider.GetRequiredService<CadastroContext>());

            // Unit of Work
            services.AddScoped<IUnitOfWork, MultiContextUnitOfWork>();

            // Health Checks
            services.AddCustomHealthChecks(connectionString);

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Handlers e Notifications
            services.AddScoped<INotificationHandler, NotificationHandler>();

            // MediatR e Validators
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(typeof(Application.Commands.Viagem.AdicionarViagemCommand).Assembly);
            });

            // Pipeline Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Application.Behaviors.UnitOfWorkBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Application.Behaviors.ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Application.Behaviors.LoggingBehavior<,>));

            // Validators
            services.AddValidatorsFromAssembly(typeof(Application.Commands.Viagem.AdicionarViagemCommand).Assembly);

            // Registro automático de serviços
            RegisterServicesFromAssembly(services, typeof(Service.Services.Localidades.LocalidadeService).Assembly);

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Registro automático de repositórios
            RegisterRepositoriesFromAssembly(services, typeof(Infra.Data.Repositories.Localidades.LocalidadeRepository).Assembly);

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registra o middleware de exceções globais
            services.AddTransient<GlobalExceptionMiddleware>();

            // Registra o ValidationBehavior do MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Configuração do Cache Distribuído
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
                options.InstanceName = "RGRTransporte:";
            });

            return services;
        }

        private static void RegisterServicesFromAssembly(IServiceCollection services, Assembly assembly)
        {
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"));

            foreach (var serviceType in serviceTypes)
            {
                var interfaceType = serviceType.GetInterfaces()
                    .FirstOrDefault(i => i.Name.EndsWith(serviceType.Name));

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, serviceType);
                }
            }
        }

        private static void RegisterRepositoriesFromAssembly(IServiceCollection services, Assembly assembly)
        {
            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"));

            foreach (var repositoryType in repositoryTypes)
            {
                var interfaceType = repositoryType.GetInterfaces()
                    .FirstOrDefault(i => i.Name.EndsWith(repositoryType.Name));

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, repositoryType);
                }
            }
        }
    }
}
