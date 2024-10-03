using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Dominio.Interfaces.Service.Viagens;
using Infra.CrossCutting.Handlers.Notifications;
using System.Net;

namespace Service.Services.Viagens
{
    public class ViagemPassageiroService : IViagemPassageiroService
    {
        private readonly IViagemPassageiroRepository _viagemPassageiroRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;

        public ViagemPassageiroService(IViagemPassageiroRepository viagemPassageiroRepository, IMapper mapper, INotificationHandler notificationHandler)
        {
            _viagemPassageiroRepository = viagemPassageiroRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
        }

        public async Task<IEnumerable<ViagemPassageiroDto>> ObterTodosAsync()
        {
            var viagemPassageiros = await _viagemPassageiroRepository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<ViagemPassageiroDto>>(viagemPassageiros);
        }

        public async Task<ViagemPassageiroDto> ObterPorIdAsync(long id)
        {
            var viagemPassageiro = await _viagemPassageiroRepository.ObterPorIdAsync(id);
            return _mapper.Map<ViagemPassageiroDto>(viagemPassageiro);
        }

        public async Task AdicionarAsync(ViagemPassageiroDto dto)
        {
            var viagemPassageiro = _mapper.Map<ViagemPassageiro>(dto);
            await _viagemPassageiroRepository.AdicionarAsync(viagemPassageiro);
        }

        public async Task EditarAsync(ViagemPassageiroDto dto)
        {
            var viagemPassageiro = _mapper.Map<ViagemPassageiro>(dto);
            await _viagemPassageiroRepository.AtualizarAsync(viagemPassageiro);
        }

        public async Task RemoverAsync(long id)
        {
            var viagemPassageiro = await _viagemPassageiroRepository.ObterPorIdAsync(id);

            if (viagemPassageiro == null)
            {
                _notificationHandler.AddNotification("ViagemPassageiro não encontrado", HttpStatusCode.NotFound);
                return;
            }

            await _viagemPassageiroRepository.RemoverAsync(viagemPassageiro);
        }
    }
}