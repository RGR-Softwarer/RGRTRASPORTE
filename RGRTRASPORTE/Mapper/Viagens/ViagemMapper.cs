using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;

namespace RGRTRASPORTE.Mapper.Viagens
{
    public class ViagemMapper : Profile
    {
        public ViagemMapper()
        {
            CreateMap<Viagem, ViagemDto>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.DataViagem, opt => opt.MapFrom(src => src.DataViagem))
                .ForMember(dest => dest.HorarioSaida, opt => opt.MapFrom(src => src.HorarioSaida))
                .ForMember(dest => dest.HorarioChegada, opt => opt.MapFrom(src => src.HorarioChegada))
                .ForMember(dest => dest.VeiculoId, opt => opt.MapFrom(src => src.VeiculoId))
                .ForMember(dest => dest.MotoristaId, opt => opt.MapFrom(src => src.MotoristaId))
                .ForMember(dest => dest.LocalidadeOrigemId, opt => opt.MapFrom(src => src.LocalidadeOrigemId))
                .ForMember(dest => dest.LocalidadeDestinoId, opt => opt.MapFrom(src => src.LocalidadeDestinoId))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao))
                .ForMember(dest => dest.Lotado, opt => opt.MapFrom(src => src.Lotado))
                .ForMember(dest => dest.DataInicioViagem, opt => opt.MapFrom(src => src.DataInicioViagem))
                .ForMember(dest => dest.DataFimViagem, opt => opt.MapFrom(src => src.DataFimViagem))
                .ForMember(dest => dest.ValorPassagem, opt => opt.MapFrom(src => src.ValorPassagem))
                .ForMember(dest => dest.QuantidadeVagas, opt => opt.MapFrom(src => src.QuantidadeVagas))
                .ForMember(dest => dest.VagasDisponiveis, opt => opt.MapFrom(src => src.VagasDisponiveis))
                .ForMember(dest => dest.Distancia, opt => opt.MapFrom(src => src.Distancia))
                .ForMember(dest => dest.DescricaoViagem, opt => opt.MapFrom(src => src.DescricaoViagem))
                .ForMember(dest => dest.PolilinhaRota, opt => opt.MapFrom(src => src.PolilinhaRota))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.GatilhoViagemId, opt => opt.MapFrom(src => src.GatilhoViagemId));
        }
    }
}