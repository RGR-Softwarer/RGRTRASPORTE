using AutoMapper;
using Application.Queries.Veiculo.Models;
using Dominio.Entidades.Veiculos;
using Dominio.Enums.Veiculo;

namespace RGRTRASPORTE.Mapper
{
    public class VeiculoMapper : Profile
    {
        public VeiculoMapper()
        {
            CreateMap<Veiculo, VeiculoDto>()
                .ForMember(dest => dest.Placa, opt => opt.MapFrom(src => src.Placa.Numero))
                .ForMember(dest => dest.PlacaFormatada, opt => opt.MapFrom(src => src.Placa.Formatada))
                .ForMember(dest => dest.TipoCombustivelDescricao, opt => opt.MapFrom(src => ObterDescricaoTipoCombustivel(src.TipoCombustivel)))
                .ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => ObterDescricaoStatus(src.Status)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Status != StatusVeiculoEnum.Inativo))
                .ForMember(dest => dest.Capacidade, opt => opt.MapFrom(src => 0))
                .ReverseMap();
        }

        private static string ObterDescricaoTipoCombustivel(TipoCombustivelEnum tipo)
        {
            return tipo switch
            {
                TipoCombustivelEnum.Gasolina => "Gasolina",
                TipoCombustivelEnum.Diesel => "Diesel",
                TipoCombustivelEnum.Eletrico => "Elétrico",
                TipoCombustivelEnum.Hibrido => "Híbrido",
                _ => tipo.ToString()
            };
        }

        private static string ObterDescricaoStatus(StatusVeiculoEnum status)
        {
            return status switch
            {
                StatusVeiculoEnum.Inativo => "Inativo",
                StatusVeiculoEnum.EmManutencao => "Em Manutenção",
                _ => status.ToString()
            };
        }
    }
}
