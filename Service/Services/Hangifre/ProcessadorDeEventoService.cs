using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Multitenancy;
//using MediatR;

namespace Service.Services.Hangifre
{
    public class ProcessadorDeEventoService
    {
        //private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;
        private readonly IGenericRepository<Viagem> _viagemRepository;

        public ProcessadorDeEventoService( 
            ITenantProvider tenantProvider, 
            IGenericRepository<Viagem> viagemRepository)
        {
            //_mediator = mediator;
            _tenantProvider = tenantProvider;
            _viagemRepository = viagemRepository;
        }

        public async Task ProcessarViagemCriada(ViagemCriadaJobData data)
        {
            var viagem = await _viagemRepository.ObterPorIdAsync(data.ViagemId);
            Console.WriteLine($"Processando viagem criada: {data.ViagemId}");
            await Task.CompletedTask;
        }

        public async Task ProcessarViagemAtualizada(ViagemJobDataBase data)
        {
            var viagem = await _viagemRepository.ObterPorIdAsync(data.ViagemId);
            Console.WriteLine($"Processando viagem atualizada: {data.ViagemId}");
            await Task.CompletedTask;
        }

        public async Task ProcessarViagemRemovida(ViagemJobDataBase data)
        {
            Console.WriteLine($"Processando viagem removida: {data.ViagemId}");
            await Task.CompletedTask;
        }

        public async Task ProcessarViagemCancelada(ViagemJobDataBase data)
        {
            var viagem = await _viagemRepository.ObterPorIdAsync(data.ViagemId);
            Console.WriteLine($"Processando viagem cancelada: {data.ViagemId}");
            await Task.CompletedTask;
        }

        public async Task ProcessarViagemIniciada(ViagemJobDataBase data)
        {
            var viagem = await _viagemRepository.ObterPorIdAsync(data.ViagemId);
            Console.WriteLine($"Processando viagem iniciada: {data.ViagemId}");
            await Task.CompletedTask;
        }

        public async Task ProcessarViagemFinalizada(ViagemJobDataBase data)
        {
            var viagem = await _viagemRepository.ObterPorIdAsync(data.ViagemId);
            Console.WriteLine($"Processando viagem finalizada: {data.ViagemId}");
            await Task.CompletedTask;
        }

        public async Task ProcessarEventoAsync()
        {
            // ... existing code ...
            await Task.CompletedTask;
        }
    }
}