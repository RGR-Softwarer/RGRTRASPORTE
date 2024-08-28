using AutoMapper;
using Dominio.Dtos.Veiculo;
using Dominio.Entidades.Veiculo;

namespace RGRTRASPORTE.Mapper
{
    public class ModeloVeicularMapper : Profile
    {
        public ModeloVeicularMapper()
        {
            CreateMap<ModeloVeicular, ModeloVeicularDto>().ReverseMap();
        }
    }
}
