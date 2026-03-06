using Application.Queries.Viagem.Models;
using ViagemEntity = Dominio.Entidades.Viagens.Viagem;

namespace Application.Queries.Viagem.Mappers;

/// <summary>
/// Mapper manual para ViagemDto seguindo princípios CQRS.
/// Em CQRS, o mapeamento deve ser explícito e controlado pelo Query Handler.
/// </summary>
public static class ViagemDtoMapper
{
    public static ViagemDto ToDto(this ViagemEntity viagem)
    {
        if (viagem == null)
            throw new ArgumentNullException(nameof(viagem));

        // Combina Data + HoraSaida para DataViagem completo
        var dataViagemCompleta = viagem.Periodo.Data.Date.Add(viagem.Periodo.HoraSaida);

        return new ViagemDto
        {
            Id = viagem.Id,
            Codigo = viagem.Codigo?.Valor ?? string.Empty,
            DataViagem = dataViagemCompleta,
            Status = viagem.Situacao.ToString(),
            Situacao = viagem.Situacao.ToString(),
            TipoTrecho = viagem.TipoTrecho,
            ViagemParId = viagem.ViagemParId,
            Origem = viagem.LocalidadeOrigem?.Nome ?? string.Empty,
            Destino = viagem.LocalidadeDestino?.Nome ?? string.Empty,
            LocalidadeOrigemNome = viagem.LocalidadeOrigem?.Nome ?? string.Empty,
            LocalidadeDestinoNome = viagem.LocalidadeDestino?.Nome ?? string.Empty,
            QuantidadeVagas = viagem.QuantidadeVagas,
            VagasDisponiveis = viagem.VagasDisponiveis,
            Veiculo = viagem.Veiculo != null ? new VeiculoViagemDto
            {
                Id = viagem.Veiculo.Id,
                Placa = viagem.Veiculo.Placa.Numero,
                Modelo = viagem.Veiculo.Modelo
            } : null,
            Motorista = viagem.Motorista != null ? new MotoristaViagemDto
            {
                Id = viagem.Motorista.Id,
                Nome = viagem.Motorista.Nome,
                Documento = viagem.Motorista.CPF.Numero
            } : null,
            Passageiros = viagem.Passageiros.Select(p => new PassageiroViagemDto
            {
                Id = p.Passageiro?.Id ?? 0,
                Nome = p.Passageiro?.Nome ?? string.Empty,
                Documento = p.Passageiro?.CPF.Numero ?? string.Empty
            }).ToList()
        };
    }

    public static IEnumerable<ViagemDto> ToDto(this IEnumerable<ViagemEntity> viagens)
    {
        return viagens.Select(v => v.ToDto());
    }
}

