using AutoMapper;
using Application.Queries.Viagem.Models;
using Dominio.Entidades.Viagens;

namespace RGRTRASPORTE.Mapper.Viagens
{
    public class ViagemMapper : Profile
    {
        public ViagemMapper()
        {
            CreateMap<Viagem, ViagemDto>()
                // Mapeamento do Value Object CodigoViagem
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Codigo.Valor))
                
                // Mapeamento das propriedades do PeriodoViagem
                .ForMember(dest => dest.DataViagem, opt => opt.MapFrom(src => src.Periodo.Data))                
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Situacao)) 
                .ReverseMap();
        }
    }
}
