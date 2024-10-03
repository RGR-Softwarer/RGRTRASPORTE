using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;

namespace RGRTRASPORTE.Mapper.Viagens
{
    public class ViagemPosicaoMapper : Profile
    {
        public ViagemPosicaoMapper()
        {
            CreateMap<ViagemPosicao, ViagemPosicaoDto>().ReverseMap();
        }
    }
}