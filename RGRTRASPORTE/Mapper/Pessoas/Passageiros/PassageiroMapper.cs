using Application.Queries.Passageiros.Models;
using AutoMapper;
using Dominio.Entidades.Pessoas.Passageiros;

namespace RGRTRASPORTE.Mapper.Pessoas.Passageiros
{
    public class PassageiroMapper : Profile
    {
        public PassageiroMapper()
        {
            CreateMap<Passageiro, PassageiroDto>()
                .ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.CPF))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Localidade.DescricaoAuditoria.ToString()))
                .ReverseMap();
        }
    }
}
