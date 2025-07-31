using AutoMapper;
using Dominio.Dtos.Viagens;
using Dominio.Entidades.Viagens;

namespace RGRTRASPORTE.Mapper.Viagens
{
    public class ViagemMapper : Profile
    {
        public ViagemMapper()
        {
            CreateMap<Viagem, ViagemDto>()
                // Mapeamento do Value Object CodigoViagem
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo.Valor))
                
                // Mapeamento das propriedades do PeriodoViagem
                .ForMember(dest => dest.DataViagem, opt => opt.MapFrom(src => src.Periodo.Data))
                .ForMember(dest => dest.HorarioSaida, opt => opt.MapFrom(src => src.Periodo.HoraSaida))
                .ForMember(dest => dest.HorarioChegada, opt => opt.MapFrom(src => src.Periodo.HoraChegada))
                
                // Propriedades diretas
                .ForMember(dest => dest.VeiculoId, opt => opt.MapFrom(src => src.VeiculoId))
                .ForMember(dest => dest.MotoristaId, opt => opt.MapFrom(src => src.MotoristaId))
                .ForMember(dest => dest.LocalidadeOrigemId, opt => opt.MapFrom(src => src.LocalidadeOrigemId))
                .ForMember(dest => dest.LocalidadeDestinoId, opt => opt.MapFrom(src => src.LocalidadeDestinoId))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.Situacao))
                .ForMember(dest => dest.Lotado, opt => opt.MapFrom(src => src.Lotado))
                .ForMember(dest => dest.DataInicioViagem, opt => opt.MapFrom(src => src.DataInicioViagem))
                .ForMember(dest => dest.DataFimViagem, opt => opt.MapFrom(src => src.DataFimViagem))
                
                // REMOVIDO: ValorPassagem não existe mais na entidade Viagem
                
                .ForMember(dest => dest.QuantidadeVagas, opt => opt.MapFrom(src => src.QuantidadeVagas))
                .ForMember(dest => dest.VagasDisponiveis, opt => opt.MapFrom(src => src.VagasDisponiveis))
                
                // Mapeamento dos Value Objects para propriedades primitivas no DTO
                .ForMember(dest => dest.DistanciaQuilometros, opt => opt.MapFrom(src => src.Distancia.Quilometros))
                .ForMember(dest => dest.DescricaoViagem, opt => opt.MapFrom(src => src.DescricaoViagem))
                .ForMember(dest => dest.PolilinhaRota, opt => opt.MapFrom(src => src.PolilinhaRota.Rota))
                
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.GatilhoViagemId, opt => opt.MapFrom(src => src.GatilhoViagemId))
                
                // Mapeamento das propriedades de auditoria da BaseEntity
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
        }
    }
}
