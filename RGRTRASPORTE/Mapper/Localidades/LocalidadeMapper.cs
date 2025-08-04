using AutoMapper;
using Application.Queries.Localidade.Models;
using Dominio.Entidades.Localidades;

namespace RGRTRASPORTE.Mapper.Localidades
{
    public class LocalidadeMapper : Profile
    {
        public LocalidadeMapper()
        {
            CreateMap<Localidade, LocalidadeDto>()
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => "Brasil"))
                .ReverseMap();

            CreateMap<Localidade, LocalidadeListagemDto>();
        }
    }
}
