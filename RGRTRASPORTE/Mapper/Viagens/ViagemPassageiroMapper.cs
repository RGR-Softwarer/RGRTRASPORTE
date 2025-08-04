using AutoMapper;
using Application.Queries.Viagem.Models;
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
                .ReverseMap();
        }
    }
}
