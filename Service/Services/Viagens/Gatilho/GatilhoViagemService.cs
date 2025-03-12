using AutoMapper;
using Dominio.Dtos.Viagens.Gatilho;
using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;
using Dominio.Interfaces.Service.Viagens.Gatilho;
using Infra.CrossCutting.Handlers.Notifications;
using System.Net;

namespace Service.Services.Viagens.Gatilho
{
    public class GatilhoViagemService : IGatilhoViagemService
    {
        private readonly IGatilhoViagemRepository _gatilhoViagemRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;

        public GatilhoViagemService(IGatilhoViagemRepository gatinhoViagemRepository, IMapper mapper, INotificationHandler notificationHandler)
        {
            _gatilhoViagemRepository = gatinhoViagemRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
        }

        public async Task<IEnumerable<GatilhoViagemDto>> ObterTodosAsync()
        {
            var gatilhoViagens = await _gatilhoViagemRepository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<GatilhoViagemDto>>(gatilhoViagens);
        }

        public async Task<GatilhoViagemDto> ObterPorIdAsync(long id)
        {
            var gatilhoViagem = await _gatilhoViagemRepository.ObterPorIdAsync(id);
            return _mapper.Map<GatilhoViagemDto>(gatilhoViagem);
        }

        public async Task AdicionarAsync(GatilhoViagemDto dto)
        {
            var gatilhoViagem = _mapper.Map<GatilhoViagem>(dto);
            await _gatilhoViagemRepository.AdicionarAsync(gatilhoViagem);
        }

        public async Task EditarAsync(GatilhoViagemDto dto)
        {
            var gatilhoViagem = _mapper.Map<GatilhoViagem>(dto);
            await _gatilhoViagemRepository.AtualizarAsync(gatilhoViagem);
        }

        public async Task RemoverAsync(long id)
        {
            var gatinhoViagem = await _gatilhoViagemRepository.ObterPorIdAsync(id);

            if (gatinhoViagem == null)
            {
                _notificationHandler.AddNotification("GatinhoViagem não encontrada", HttpStatusCode.NotFound);
                return;
            }

            await _gatilhoViagemRepository.RemoverAsync(gatinhoViagem);
        }
    }
}