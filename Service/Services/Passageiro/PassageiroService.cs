using AutoMapper;
using Dominio.Dtos.Pessoas.Passageiros;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Interfaces.Infra.Data.Passageiros;
using Dominio.Interfaces.Service.Passageiros;
using Infra.CrossCutting.Handlers.Notifications;
using System.Net;

namespace Service.Services.Pessoas.Passageiros
{
    public class PassageiroService : IPassageiroService
    {
        private readonly IPassageiroRepository _passageiroRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;


        public PassageiroService(IPassageiroRepository passageiroRepository, IMapper mapper, INotificationHandler notificationHandler)
        {
            _passageiroRepository = passageiroRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
        }

        public async Task<IEnumerable<PassageiroDto>> ObterTodosAsync()
        {
            var passageiros = await _passageiroRepository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<PassageiroDto>>(passageiros);
        }

        public async Task<PassageiroDto> ObterPorIdAsync(long id, bool auditado = false)
        {
            var passageiro = await _passageiroRepository.ObterPorIdAsync(id, auditado);
            return _mapper.Map<PassageiroDto>(passageiro);
        }

        public async Task AdicionarAsync(PassageiroDto dto)
        {
            var passageiro = _mapper.Map<Passageiro>(dto);
            await _passageiroRepository.AdicionarAsync(passageiro);
        }

        public async Task EditarAsync(PassageiroDto dto)
        {
            var passageiro = _mapper.Map<Passageiro>(dto);
            await _passageiroRepository.AtualizarAsync(passageiro);
        }

        public async Task RemoverAsync(long id)
        {
            var passageiro = await _passageiroRepository.ObterPorIdAsync(id);

            if (passageiro == null)
            {
                _notificationHandler.AddNotification("Passageiro não encontrado", HttpStatusCode.NotFound);
                return;
            }

            await _passageiroRepository.RemoverAsync(passageiro);
        }
    }
}