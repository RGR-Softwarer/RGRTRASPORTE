using AutoMapper;
using Dominio.Dtos.Localidades;
using Dominio.Entidades.Localidades;
//Ajustar
namespace RGRTRASPORTE.Mapper.Localidades
{
    public class LocalidadeMapper : Profile
    {
        public LocalidadeMapper()
        {
            Console.WriteLine("LocalidadeMapper carregado.");

            CreateMap<Localidade, LocalidadeDto>().ReverseMap();
        }
    }
}
