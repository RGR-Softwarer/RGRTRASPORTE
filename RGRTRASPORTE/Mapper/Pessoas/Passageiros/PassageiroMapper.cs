using AutoMapper;
using Dominio.Dtos.Pessoas.Passageiros;
using Dominio.Entidades.Pessoas.Passageiros;

namespace RGRTRASPORTE.Mapper.Pessoas.Passageiros
{
    public class PassageiroMapper : Profile
    {
        public PassageiroMapper()
        {
            CreateMap<Passageiro, PassageiroDto>().ReverseMap();
        }
    }
}