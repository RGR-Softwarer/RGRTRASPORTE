using AutoMapper;
using Dominio.Dtos.Viagens.Gatilho;
using Dominio.Entidades.Viagens.Gatilho;
//Ajustar
namespace RGRTRASPORTE.Mapper.Viagens.Gatilho
{
    public class GatilhoViagemMapper : Profile
    {
        public GatilhoViagemMapper()
        {
            CreateMap<GatilhoViagem, GatilhoViagemDto>().ReverseMap();
        }
    }
}
