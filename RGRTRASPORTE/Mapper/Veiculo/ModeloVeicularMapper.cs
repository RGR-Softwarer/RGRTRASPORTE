using AutoMapper;
using Application.Queries.Veiculo.ModeloVeicular.Models;
using Dominio.Entidades.Veiculos;
using Dominio.Enums.Veiculo;

namespace RGRTRASPORTE.Mapper
{
    public class ModeloVeicularMapper : Profile
    {
        public ModeloVeicularMapper()
        {
            CreateMap<ModeloVeicular, ModeloVeicularDto>()
                .ForMember(dest => dest.DescricaoModelo, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.SituacaoDescricao, opt => opt.MapFrom(src => src.Situacao ? "Ativo" : "Inativo"))
                .ForMember(dest => dest.TipoDescricao, opt => opt.MapFrom(src => src.Tipo.ObterDescricao()))
                .ForMember(dest => dest.PossuiBanheiroDescricao, opt => opt.MapFrom(src => src.PossuiBanheiro ? "Sim" : "Não"))
                .ForMember(dest => dest.PossuiClimatizadorDescricao, opt => opt.MapFrom(src => src.PossuiClimatizador ? "Sim" : "Não"))
                .ReverseMap();
        }
    }
}
