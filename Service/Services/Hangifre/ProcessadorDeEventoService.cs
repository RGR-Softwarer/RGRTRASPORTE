using Dominio.Dtos.Viagens;
using Dominio.Interfaces.Hangfire;
using Infra.CrossCutting.Multitenancy;
//using MediatR;

namespace Service.Services.Hangifre
{
    public class ProcessadorDeEventoService : IProcessadorDeEventoService
    {
        //private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        public ProcessadorDeEventoService( ITenantProvider tenantProvider)
        {
            //_mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        public async Task ProcessarViagemCriada(ViagemCriadaJobData data)
        {
            _tenantProvider.SetTenantId(data.TenantId);

            Console.WriteLine("Processando");

            //await _mediator.Send(new ProcessarViagemCriadaCommand
            //{
            //    ViagemId = data.ViagemId
            //});
        }

        public async Task ProcessarEventoAsync()
        {
            // ... existing code ...
            await Task.CompletedTask;
        }
    }
}