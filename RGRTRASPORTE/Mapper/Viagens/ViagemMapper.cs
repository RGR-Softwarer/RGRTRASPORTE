using AutoMapper;
using Application.Queries.Viagem.Models;
using Dominio.Entidades.Viagens;

namespace RGRTRASPORTE.Mapper.Viagens
{
    public class ViagemMapper : Profile
    {
        public ViagemMapper()
        {
            CreateMap<Viagem, ViagemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo != null ? src.Codigo.Valor : string.Empty))
                .ForMember(dest => dest.DataViagem, opt => opt.MapFrom(src => src.Periodo.Data))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Situacao.ToString()))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao.ToString()))
                .ForMember(dest => dest.TipoTrecho, opt => opt.MapFrom(src => src.TipoTrecho))
                .ForMember(dest => dest.ViagemParId, opt => opt.MapFrom(src => src.ViagemParId))
                .ForMember(dest => dest.Origem, opt => opt.MapFrom(src => src.LocalidadeOrigem != null ? src.LocalidadeOrigem.Nome : string.Empty))
                .ForMember(dest => dest.Destino, opt => opt.MapFrom(src => src.LocalidadeDestino != null ? src.LocalidadeDestino.Nome : string.Empty))
                .ForMember(dest => dest.LocalidadeOrigemNome, opt => opt.MapFrom(src => src.LocalidadeOrigem != null ? src.LocalidadeOrigem.Nome : string.Empty))
                .ForMember(dest => dest.LocalidadeDestinoNome, opt => opt.MapFrom(src => src.LocalidadeDestino != null ? src.LocalidadeDestino.Nome : string.Empty))
                .ForMember(dest => dest.QuantidadeVagas, opt => opt.MapFrom(src => src.QuantidadeVagas))
                .ForMember(dest => dest.VagasDisponiveis, opt => opt.MapFrom(src => src.VagasDisponiveis))
                .ForMember(dest => dest.Veiculo, opt => opt.MapFrom(src => src.Veiculo != null ? new VeiculoViagemDto
                {
                    Id = src.Veiculo.Id,
                    Placa = src.Veiculo.Placa.Numero,
                    Modelo = src.Veiculo.Modelo
                } : null))
                .ForMember(dest => dest.Motorista, opt => opt.MapFrom(src => src.Motorista != null ? new MotoristaViagemDto
                {
                    Id = src.Motorista.Id,
                    Nome = src.Motorista.Nome,
                    Documento = src.Motorista.CPF.Numero
                } : null))
                .ForMember(dest => dest.Passageiros, opt => opt.MapFrom(src => src.Passageiros.Select(p => new PassageiroViagemDto
                {
                    Id = p.Passageiro != null ? p.Passageiro.Id : 0,
                    Nome = p.Passageiro != null ? p.Passageiro.Nome : string.Empty,
                    Documento = p.Passageiro != null ? p.Passageiro.CPF.Numero : string.Empty
                }).ToList()))
                .ForMember(dest => dest.ValorViagem, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
