using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Dominio.Interfaces.Service.Viagens;
using Infra.CrossCutting.Handlers.Notifications;
using System.Net;

namespace Service.Services.Viagens
{
    public class ViagemService : IViagemService
    {
        private readonly IViagemRepository _viagemRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;

        public ViagemService(IViagemRepository viagemRepository, IMapper mapper, INotificationHandler notificationHandler)
        {
            _viagemRepository = viagemRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
        }

        public async Task<IEnumerable<ViagemDto>> ObterTodosAsync()
        {
            var viagens = await _viagemRepository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<ViagemDto>>(viagens);
        }

        public async Task<ViagemDto> ObterPorIdAsync(long id)
        {
            var viagem = await _viagemRepository.ObterPorIdAsync(id);
            return _mapper.Map<ViagemDto>(viagem);
        }

        public async Task AdicionarAsync(ViagemDto dto)
        {
            var viagem = _mapper.Map<Viagem>(dto);
            await _viagemRepository.AdicionarAsync(viagem);
        }

        public async Task EditarAsync(ViagemDto dto)
        {
            var viagem = _mapper.Map<Viagem>(dto);
            await _viagemRepository.AtualizarAsync(viagem);
        }

        public async Task RemoverAsync(long id)
        {
            var viagem = await _viagemRepository.ObterPorIdAsync(id);

            if (viagem == null)
            {
                _notificationHandler.AddNotification("Viagem não encontrada", HttpStatusCode.NotFound);
                return;
            }

           await _viagemRepository.RemoverAsync(viagem);
        }
    }
}