using AutoMapper;
using Dominio.Dtos.Localidades;
using Dominio.Entidades.Localidades;
using Dominio.Interfaces.Infra.Data.Localidades;
using Dominio.Interfaces.Service.Localidades;
using Infra.CrossCutting.Handlers.Notifications;
using System.Net;

namespace Service.Services.Localidades
{
    public class LocalidadeService : ILocalidadeService
    {
        private readonly ILocalidadeRepository _localidadeRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;

        public LocalidadeService(ILocalidadeRepository localidadeRepository, IMapper mapper, INotificationHandler notificationHandler)
        {
            _localidadeRepository = localidadeRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
        }

        public async Task<IEnumerable<LocalidadeDto>> ObterTodosAsync()
        {
            var localidades = await _localidadeRepository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<LocalidadeDto>>(localidades);
        }

        public async Task<LocalidadeDto> ObterPorIdAsync(long id, bool auditado = false)
        {
            var localidade = await _localidadeRepository.ObterPorIdAsync(id, auditado);
            return _mapper.Map<LocalidadeDto>(localidade);
        }

        public async Task AdicionarAsync(LocalidadeDto dto)
        {
            var localidade = _mapper.Map<Localidade>(dto);
            await _localidadeRepository.AdicionarAsync(localidade);
        }

        public async Task EditarAsync(LocalidadeDto dto)
        {
            var localidade = _mapper.Map<Localidade>(dto);
            await _localidadeRepository.AtualizarAsync(localidade);
        }

        public async Task RemoverAsync(long id)
        {
            var localidade = await _localidadeRepository.ObterPorIdAsync(id);

            if (localidade == null)
            {
                _notificationHandler.AddNotification("Localidade não encontrada", HttpStatusCode.NotFound);
                return;
            }

            await _localidadeRepository.RemoverAsync(localidade);
        }
    }
}