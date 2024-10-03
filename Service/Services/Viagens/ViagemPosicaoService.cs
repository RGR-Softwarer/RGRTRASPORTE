using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Dominio.Interfaces.Service.Viagens;
using Infra.CrossCutting.Handlers.Notifications;
using System.Net;

namespace Service.Services.Viagens
{
    public class ViagemPosicaoService : IViagemPosicaoService
    {
        private readonly IViagemPosicaoRepository _viagemPosicaoRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;

        public ViagemPosicaoService(IViagemPosicaoRepository viagemPosicaoRepository, IMapper mapper, INotificationHandler notificationHandler)
        {
            _viagemPosicaoRepository = viagemPosicaoRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
        }

        public async Task<IEnumerable<ViagemPosicaoDto>> ObterTodosAsync()
        {
            var viagemPosicoes = await _viagemPosicaoRepository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<ViagemPosicaoDto>>(viagemPosicoes);
        }

        public async Task<ViagemPosicaoDto> ObterPorIdAsync(long id)
        {
            var viagemPosicao = await _viagemPosicaoRepository.ObterPorIdAsync(id);
            return _mapper.Map<ViagemPosicaoDto>(viagemPosicao);
        }

        public async Task AdicionarAsync(ViagemPosicaoDto dto)
        {
            var viagemPosicao = _mapper.Map<ViagemPosicao>(dto);
            await _viagemPosicaoRepository.AdicionarAsync(viagemPosicao);
        }

        public async Task EditarAsync(ViagemPosicaoDto dto)
        {
            var viagemPosicao = _mapper.Map<ViagemPosicao>(dto);
            await _viagemPosicaoRepository.AtualizarAsync(viagemPosicao);
        }

        public async Task RemoverAsync(long id)
        {
            var viagemPosicao = await _viagemPosicaoRepository.ObterPorIdAsync(id);

            if (viagemPosicao == null)
            {
                _notificationHandler.AddNotification("ViagemPosicao não encontrada", HttpStatusCode.NotFound);
                return;
            }

            await _viagemPosicaoRepository.RemoverAsync(viagemPosicao);
        }
    }
}