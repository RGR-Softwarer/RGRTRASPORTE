using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;

namespace RGRTRASPORTE.Mapper.Viagens
{
    public class ViagemPassageiroMapper : Profile
    {
        public ViagemPassageiroMapper()
        {
            CreateMap<ViagemPassageiro, ViagemPassageiroDto>().ReverseMap();
        }
    }
}