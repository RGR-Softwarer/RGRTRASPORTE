using AutoMapper;
using Dominio.Dtos;
using Dominio.Entidades;
using Dominio.Interfaces.Infra.Data;
using Dominio.Interfaces.Service;
using Infra.CrossCutting.Handlers.Notifications;

namespace Service
{
    public class VeiculoService : ServiceBase, IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IMapper _mapper;

        public VeiculoService(INotificationHandler notificationHandler, IVeiculoRepository veiculoRepository, IMapper mapper) : base(notificationHandler)
        {
            _veiculoRepository = veiculoRepository;
            _mapper = mapper;
        }

        public async Task<List<VeiculoDto>> ObterTodosAsync()
        {
            var veiculos = await _veiculoRepository.ObterTodosAsync();

            if (veiculos?.Count > 0)
                return _mapper.Map<List<VeiculoDto>>(veiculos);

            return new List<VeiculoDto>();
        }

        public async Task<VeiculoDto> ObterPorIdAsync(int id)
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
            await _veiculoRepository.AdicionarAsync(veiculo);
        }

        public async Task AdicionarEmLoteAsync(List<VeiculoDto> dto)
        {
            var veiculo = _mapper.Map<List<Veiculo>>(dto);
            await _veiculoRepository.AdicionarEmLoteAsync(veiculo);
        }

        public void EditarAsync(VeiculoDto dto)
        {
            if (dto != null)
            {
                 _veiculoRepository.Editar(_mapper.Map<Veiculo>(dto));
            }
        }

        public void EditarEmLoteAsync(List<VeiculoDto> dto)
        {
            if (dto.Count > 0)
                 _veiculoRepository.EditarEmLoteAsync(_mapper.Map<List<Veiculo>>(dto));
        }

        public async Task RemoverAsync(int id)
        {
            var veiculo = await _veiculoRepository.ObterPorIdAsync(id);
            if (veiculo != null)
                _veiculoRepository.Remover(veiculo);
        }
    }
}
