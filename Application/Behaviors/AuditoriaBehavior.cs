using Dominio.Entidades;
using Dominio.Enums.Auditoria;
using Dominio.Services;
using Infra.CrossCutting.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Behaviors
{
    public class AuditoriaBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AuditoriaBehavior<TRequest, TResponse>> _logger;

        public AuditoriaBehavior(
            IAuditoriaService auditoriaService,
            ICurrentUserService currentUserService,
            ILogger<AuditoriaBehavior<TRequest, TResponse>> logger)
        {
            _auditoriaService = auditoriaService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            var nomeComando = typeof(TRequest).Name;
            var isComandoDeEscrita = nomeComando.Contains("Criar") || 
                                   nomeComando.Contains("Atualizar") || 
                                   nomeComando.Contains("Remover") ||
                                   nomeComando.Contains("Editar") ||
                                   nomeComando.Contains("Deletar");

            if (!isComandoDeEscrita)
            {
                return await next();
            }

            var dadosRequest = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var usuario = _currentUserService.UserName ?? "Sistema";
            var ip = _currentUserService.IpAddress ?? "Unknown";

            try
            {
                var response = await next();

                // Registra o comando executado
                await _auditoriaService.RegistrarEventoAsync(
                    $"Comando_{nomeComando}",
                    ObterEntidadeId(request),
                    dadosRequest,
                    usuario,
                    ip
                );

                _logger.LogInformation("Comando {NomeComando} auditado pelo usuário {Usuario}", 
                    nomeComando, usuario);

                return response;
            }
            catch (Exception ex)
            {
                // Registra comando que falhou
                await _auditoriaService.RegistrarEventoAsync(
                    $"Comando_Falha_{nomeComando}",
                    ObterEntidadeId(request),
                    $"Request: {dadosRequest}\nErro: {ex.Message}",
                    usuario,
                    ip
                );

                throw;
            }
        }

        private long ObterEntidadeId(TRequest request)
        {
            // Tenta extrair o ID da entidade do request
            var propriedades = typeof(TRequest).GetProperties();
            
            var propriedadeId = propriedades.FirstOrDefault(p => 
                p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) && 
                p.PropertyType == typeof(long));

            if (propriedadeId != null)
            {
                var valor = propriedadeId.GetValue(request);
                if (valor is long id)
                    return id;
            }

            return 0;
        }
    }
} 