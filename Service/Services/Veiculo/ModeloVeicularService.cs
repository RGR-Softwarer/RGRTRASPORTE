using AutoMapper;
using Dominio.Dtos.Veiculo;
using Dominio.Entidades.Veiculo;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service;
using Infra.CrossCutting.Handlers.Notifications;
using System.Net;

namespace Service.Services
{
    public class ModeloVeicularService : ServiceBase, IModeloVeicularService
    {
        private readonly IModeloVeicularRepository _modeloVeicularRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;

        public ModeloVeicularService(INotificationHandler notificationHandler, IModeloVeicularRepository modeloVeicularRepository, IMapper mapper) : base(notificationHandler)
        {
            _modeloVeicularRepository = modeloVeicularRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
        }

        public async Task<List<ModeloVeicularDto>> ObterTodosAsync()
        {
            var modelosVeicular = await _modeloVeicularRepository.ObterTodosAsync();

            if (modelosVeicular?.Count > 0)
                return _mapper.Map<List<ModeloVeicularDto>>(modelosVeicular);

            return new List<ModeloVeicularDto>();
        }

        public async Task<ModeloVeicularDto> ObterPorIdAsync(long id)
        {
            var modeloVeicular = await _modeloVeicularRepository.ObterPorIdAsync(id);

            ModeloVeicularDto modeloVeicularDto = null;

            if (modeloVeicular != null)
                return _mapper.Map<ModeloVeicularDto>(modeloVeicular);


            return modeloVeicularDto;
        }

        public async Task AdicionarAsync(ModeloVeicularDto dto)
        {
            var modeloVeicular = _mapper.Map<ModeloVeicular>(dto);

            await _modeloVeicularRepository.AdicionarAsync(modeloVeicular, Auditado);
        }

        public void EditarAsync(ModeloVeicularDto dto)
        {
            if (dto != null)
            {
                _modeloVeicularRepository.Atualizar(_mapper.Map<ModeloVeicular>(dto), Auditado);
            }
        }

        public async Task RemoverAsync(long id)
        {
            var modeloVeicular = await _modeloVeicularRepository.ObterPorIdAsync(id);

            if (modeloVeicular == null)
            {
                _notificationHandler.AddNotification("Modelo Veicular não encontrado", HttpStatusCode.NotFound);
                return;
            }

            _modeloVeicularRepository.Remover(modeloVeicular);
        }
    }
}
