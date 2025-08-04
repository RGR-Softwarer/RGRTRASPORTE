using AutoMapper;
using Application.Queries.Viagem.Models;
using Dominio.Entidades.Viagens.Gatilho;

namespace RGRTRASPORTE.Mapper.Viagens.Gatilho
{
    public class GatilhoViagemMapper : Profile
    {
        public GatilhoViagemMapper()
        {
            CreateMap<GatilhoViagem, GatilhoViagemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.VeiculoId, opt => opt.MapFrom(src => src.VeiculoId))
                .ForMember(dest => dest.MotoristaId, opt => opt.MapFrom(src => src.MotoristaId))
                .ForMember(dest => dest.OrigemId, opt => opt.MapFrom(src => src.LocalidadeOrigemId))
                .ForMember(dest => dest.DestinoId, opt => opt.MapFrom(src => src.LocalidadeDestinoId))
                .ForMember(dest => dest.HorarioSaida, opt => opt.MapFrom(src => DateTime.Today.Add(src.HorarioSaida)))
                .ForMember(dest => dest.HorarioChegada, opt => opt.MapFrom(src => DateTime.Today.Add(src.HorarioChegada)))
                .ForMember(dest => dest.DescricaoViagem, opt => opt.MapFrom(src => src.DescricaoViagem))
                .ForMember(dest => dest.Distancia, opt => opt.MapFrom(src => src.Distancia))
                .ForMember(dest => dest.PolilinhaRota, opt => opt.MapFrom(src => src.PolilinhaRota))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.DiasSemana, opt => opt.MapFrom(src => src.DiasSemana))
                .ReverseMap();
        }
    }
}
