using AutoMapper;
using Dominio.Dtos.Veiculo;
using Dominio.Entidades.Veiculos;
using Dominio.Interfaces.Infra.Data.Veiculo;
using Dominio.Interfaces.Service;
using Dominio.Validators;
using Infra.CrossCutting.Handlers.Notifications;
using System.Net;

namespace Service.Services
{
    public class VeiculoService : ServiceBase, IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IMapper _mapper;
        private readonly INotificationHandler _notificationHandler;

        public VeiculoService(INotificationHandler notificationHandler, IVeiculoRepository veiculoRepository, IMapper mapper) : base(notificationHandler)
        {
            _veiculoRepository = veiculoRepository;
            _mapper = mapper;
            _notificationHandler = notificationHandler;
        }

        public async Task<List<VeiculoDto>> ObterTodosAsync()
        {
            var veiculos = await _veiculoRepository.ObterTodosAsync();

            if (veiculos?.Count > 0)
                return _mapper.Map<List<VeiculoDto>>(veiculos);

            return new List<VeiculoDto>();
        }

        public async Task<VeiculoDto> ObterPorIdAsync(long id)
        {
            var veiculo = await _veiculoRepository.ObterPorIdAsync(id);

            VeiculoDto veiculoDto = null;

            if (veiculo != null)
                return _mapper.Map<VeiculoDto>(veiculo);


            return veiculoDto;
        }

        public async Task AdicionarAsync(VeiculoDto dto)
        {
            var veiculo = _mapper.Map<Veiculo>(dto);

            Validar(veiculo, new VeiculoValidator());

            await _veiculoRepository.AdicionarAsync(veiculo, Auditado);
        }

        public async Task AdicionarEmLoteAsync(List<VeiculoDto> dto)
        {
            var veiculo = _mapper.Map<List<Veiculo>>(dto);
            await _veiculoRepository.AdicionarEmLoteAsync(veiculo);
        }

        public async Task EditarAsync(VeiculoDto dto)
        {
            if (dto != null)
            {
                await _veiculoRepository.AtualizarAsync(_mapper.Map<Veiculo>(dto), Auditado);
            }
        }

        public async Task EditarEmLoteAsync(List<VeiculoDto> dto)
        {
            if (dto.Count > 0)
                await _veiculoRepository.AtualizarEmLoteAsync(_mapper.Map<List<Veiculo>>(dto));
        }

        public async Task RemoverAsync(long id)
        {
            var veiculo = await _veiculoRepository.ObterPorIdAsync(id);

            if (veiculo == null)
            {
                _notificationHandler.AddNotification("Veículo não encontrado", HttpStatusCode.NotFound);
                return;
            }

           await _veiculoRepository.RemoverAsync(veiculo);
        }

    }
}
