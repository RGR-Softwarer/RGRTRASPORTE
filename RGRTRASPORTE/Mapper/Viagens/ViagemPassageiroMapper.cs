using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;

namespace RGRTRASPORTE.Mapper.Viagens
{
    public class ViagemPassageiroMapper : Profile
    {
        public ViagemPassageiroMapper()
        {
            CreateMap<ViagemPassageiro, ViagemPassageiroDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ViagemId, opt => opt.MapFrom(src => src.ViagemId))
                .ForMember(dest => dest.PassageiroId, opt => opt.MapFrom(src => src.PassageiroId))
                .ForMember(dest => dest.DataReserva, opt => opt.MapFrom(src => src.DataReserva))
                .ForMember(dest => dest.Confirmado, opt => opt.MapFrom(src => src.Confirmado))
                .ForMember(dest => dest.DataConfirmacao, opt => opt.MapFrom(src => src.DataConfirmacao));
        }
    }
}