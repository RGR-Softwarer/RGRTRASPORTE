using AutoMapper;
using Dominio.Dtos;
using Dominio.Entidades.Veiculos;

namespace RGRTRASPORTE.Mapper
{
    public class VeiculoMapper : Profile
    {
        public VeiculoMapper()
        {
            CreateMap<Veiculo, VeiculoDto>().ReverseMap();
        }
    }
}
