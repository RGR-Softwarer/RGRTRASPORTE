using Dominio.Dtos.Viagens.Gatilho;
using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Interfaces.Infra.Data.Viagens.Gatilho;
using Dominio.Interfaces.Service.Viagens.Gatilho;
using Dominio.Interfaces.Infra.Data;
using Microsoft.Extensions.Logging;

namespace Service.Services.Viagens
{
    public class GatilhoViagemService : IGatilhoViagemService
    {
        private readonly IGatilhoViagemRepository _gatilhoViagemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GatilhoViagemService> _logger;

        public GatilhoViagemService(
            IGatilhoViagemRepository gatilhoViagemRepository,
            IUnitOfWork unitOfWork,
            ILogger<GatilhoViagemService> logger)
        {
            _gatilhoViagemRepository = gatilhoViagemRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<GatilhoViagemDto>> ObterTodosAsync()
        {
            try
            {
                _logger.LogInformation("Buscando todos os gatilhos de viagem");

                var gatilhos = await _gatilhoViagemRepository.ObterTodosAsync();

                var gatilhosDto = gatilhos.Select(g => new GatilhoViagemDto
                {
                    Id = g.Id,
                    Descricao = g.Descricao,
                    VeiculoId = g.VeiculoId,
                    MotoristaId = g.MotoristaId,
                    OrigemId = g.LocalidadeOrigemId,
                    DestinoId = g.LocalidadeDestinoId,
                    HorarioSaida = DateTime.Today.Add(g.HorarioSaida),
                    HorarioChegada = DateTime.Today.Add(g.HorarioChegada),
                    DescricaoViagem = g.DescricaoViagem,
                    Distancia = g.Distancia,
                    PolilinhaRota = g.PolilinhaRota,
                    Ativo = g.Ativo,
                    DiasSemana = g.DiasSemana
                }).ToList();

                _logger.LogInformation("Gatilhos de viagem encontrados com sucesso. Total: {Total}", gatilhosDto.Count());

                return gatilhosDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar gatilhos de viagem");
                throw;
            }
        }

        public async Task<GatilhoViagemDto> ObterPorIdAsync(long id)
        {
            try
            {
                _logger.LogInformation("Buscando gatilho de viagem por ID: {Id}", id);

                var gatilho = await _gatilhoViagemRepository.ObterPorIdAsync(id);

                if (gatilho == null)
                {
                    _logger.LogWarning("Gatilho de viagem não encontrado para ID: {Id}", id);
                    return null;
                }

                var gatilhoDto = new GatilhoViagemDto
                {
                    Id = gatilho.Id,
                    Descricao = gatilho.Descricao,
                    VeiculoId = gatilho.VeiculoId,
                    MotoristaId = gatilho.MotoristaId,
                    OrigemId = gatilho.LocalidadeOrigemId,
                    DestinoId = gatilho.LocalidadeDestinoId,
                    HorarioSaida = DateTime.Today.Add(gatilho.HorarioSaida),
                    HorarioChegada = DateTime.Today.Add(gatilho.HorarioChegada),
                    DescricaoViagem = gatilho.DescricaoViagem,
                    Distancia = gatilho.Distancia,
                    PolilinhaRota = gatilho.PolilinhaRota,
                    Ativo = gatilho.Ativo,
                    DiasSemana = gatilho.DiasSemana
                };

                _logger.LogInformation("Gatilho de viagem encontrado com sucesso para ID: {Id}", id);

                return gatilhoDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar gatilho de viagem por ID: {Id}", id);
                throw;
            }
        }

        public async Task AdicionarAsync(GatilhoViagemDto dto)
        {
            try
            {
                _logger.LogInformation("Adicionando novo gatilho de viagem: {Descricao}", dto.Descricao);

                var gatilho = new GatilhoViagem(
                    dto.Descricao,
                    dto.VeiculoId,
                    dto.MotoristaId,
                    dto.OrigemId,
                    dto.DestinoId,
                    dto.HorarioSaida.TimeOfDay,
                    dto.HorarioChegada.TimeOfDay,
                    0, // valor passagem será definido na criação da viagem
                    0, // quantidade vagas será definido na criação da viagem
                    dto.Distancia,
                    dto.DescricaoViagem,
                    dto.PolilinhaRota,
                    dto.DiasSemana,
                    dto.Ativo);

                await _gatilhoViagemRepository.AdicionarAsync(gatilho);

                _logger.LogInformation("Gatilho de viagem adicionado com sucesso. ID: {Id}", gatilho.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar gatilho de viagem: {Descricao}", dto.Descricao);
                throw;
            }
        }

        public async Task EditarAsync(GatilhoViagemDto dto)
        {
            try
            {
                _logger.LogInformation("Editando gatilho de viagem ID: {Id}", dto.Id);

                var gatilho = await _gatilhoViagemRepository.ObterPorIdAsync(dto.Id);

                if (gatilho == null)
                {
                    _logger.LogWarning("Gatilho de viagem não encontrado para edição. ID: {Id}", dto.Id);
                    throw new InvalidOperationException($"Gatilho de viagem com ID {dto.Id} não encontrado");
                }

                // Atualizar propriedades do gatilho
                // Como as propriedades são private set, seria necessário métodos de atualização na entidade
                // Por enquanto, vamos criar um novo gatilho

                var novoGatilho = new GatilhoViagem(
                    dto.Descricao,
                    dto.VeiculoId,
                    dto.MotoristaId,
                    dto.OrigemId,
                    dto.DestinoId,
                    dto.HorarioSaida.TimeOfDay,
                    dto.HorarioChegada.TimeOfDay,
                    0, // valor passagem será definido na criação da viagem
                    0, // quantidade vagas será definido na criação da viagem
                    dto.Distancia,
                    dto.DescricaoViagem,
                    dto.PolilinhaRota,
                    dto.DiasSemana,
                    dto.Ativo);

                await _gatilhoViagemRepository.RemoverAsync(gatilho);
                await _gatilhoViagemRepository.AdicionarAsync(novoGatilho);

                _logger.LogInformation("Gatilho de viagem editado com sucesso. ID: {Id}", dto.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar gatilho de viagem ID: {Id}", dto.Id);
                throw;
            }
        }

        public async Task RemoverAsync(long id)
        {
            try
            {
                _logger.LogInformation("Removendo gatilho de viagem ID: {Id}", id);

                var gatilho = await _gatilhoViagemRepository.ObterPorIdAsync(id);

                if (gatilho == null)
                {
                    _logger.LogWarning("Gatilho de viagem não encontrado para remoção. ID: {Id}", id);
                    throw new InvalidOperationException($"Gatilho de viagem com ID {id} não encontrado");
                }

                await _gatilhoViagemRepository.RemoverAsync(gatilho);

                _logger.LogInformation("Gatilho de viagem removido com sucesso. ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover gatilho de viagem ID: {Id}", id);
                throw;
            }
        }
    }
} 