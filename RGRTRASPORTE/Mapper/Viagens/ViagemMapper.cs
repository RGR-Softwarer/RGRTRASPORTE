using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;

namespace RGRTRASPORTE.Mapper.Viagens
{
    public class ViagemMapper : Profile
    {
        public ViagemMapper()
        {
            CreateMap<Viagem, ViagemDto>().ReverseMap();
        }
    }
}