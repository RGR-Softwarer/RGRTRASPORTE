using AutoMapper;
using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Entidades.Veiculos;
using Dominio.Entidades.Viagens;
using Application.Queries.Localidade.Models;
using Application.Queries.Passageiro.Models;
using Application.Queries.Veiculo.Models;
using Application.Queries.Viagem.Models;
using Application.Queries.Veiculo.ModeloVeicular.Models;

namespace Application.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Viagem, ViagemDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Situacao));

        CreateMap<ViagemPosicao, ViagemPosicaoDto>();

        CreateMap<Passageiro, PassageiroDto>()
            .ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.CPF));

        CreateMap<Localidade, LocalidadeDto>()
            .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => "Brasil"));

        CreateMap<Localidade, LocalidadeListagemDto>();

        CreateMap<Veiculo, VeiculoDto>();

        CreateMap<ModeloVeicular, ModeloVeicularDto>()
            .ForMember(dest => dest.DescricaoModelo, opt => opt.MapFrom(src => src.Descricao));
    }
} 
