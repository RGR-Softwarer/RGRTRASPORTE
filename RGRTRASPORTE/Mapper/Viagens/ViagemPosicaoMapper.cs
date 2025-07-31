using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;

namespace RGRTRASPORTE.Mapper.Viagens
{
    public class ViagemPosicaoMapper : Profile
    {
        public ViagemPosicaoMapper()
        {
            CreateMap<ViagemPosicao, ViagemPosicaoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ViagemId, opt => opt.MapFrom(src => src.ViagemId))
                .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DataHora))
                // Fixed: Using proper decimal mapping instead of ToString() to preserve precision
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));
        }
    }
}
