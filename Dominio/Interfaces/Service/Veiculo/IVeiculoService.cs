﻿using Dominio.Dtos;
using Dominio.Dtos.Veiculo;

namespace Dominio.Interfaces.Service
{
    public interface IVeiculoService
    {
        Task<List<VeiculoDto>> ObterTodosAsync();
        Task<ResponseGridDto<VeiculoDto>> ObterPaginadoAsync(ParametrosBuscaDto parametros, CancellationToken cancellationToken = default);
        Task<VeiculoDto> ObterPorIdAsync(long id);
        Task AdicionarAsync(VeiculoDto dto);
        Task AdicionarEmLoteAsync(List<VeiculoDto> dto);
        Task EditarAsync(VeiculoDto dto);
        Task EditarEmLoteAsync(List<VeiculoDto> dto);
        Task RemoverAsync(long id);
    }
}
