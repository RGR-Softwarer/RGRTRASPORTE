using Application.Behaviors;
using Application.Events;
using Dominio.Services;
using Infra.CrossCutting.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Ioc
{
    public static class AuditoriaExtensions
    {
        public static IServiceCollection AddAuditoria(this IServiceCollection services)
        {
            // Serviços de auditoria
            services.AddScoped<IAuditoriaService, AuditoriaService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Event handlers
            services.AddScoped<INotificationHandler<Dominio.Events.Base.DomainEvent>, AuditoriaEventHandler>();

            // Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditoriaBehavior<,>));

            return services;
        }
    }
} 