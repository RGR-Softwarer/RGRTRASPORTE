using Dominio.Entidades.Viagens;
using Dominio.Enums.Viagens;

namespace Dominio.Specifications
{
    // Specifications básicas para consultas simples - mantendo apenas as que não têm duplicação
    public class ViagemPodeSerIniciadaSpecification : Specification<Viagem>
    {
        public override bool IsSatisfiedBy(Viagem viagem)
        {
            return viagem.Situacao == SituacaoViagemEnum.Agendada;
        }

        public override string ErrorMessage => 
            "Apenas viagens agendadas podem ser iniciadas";
    }

    public class ViagemPodeSerFinalizadaSpecification : Specification<Viagem>
    {
        public override bool IsSatisfiedBy(Viagem viagem)
        {
            return viagem.Situacao == SituacaoViagemEnum.EmAndamento;
        }

        public override string ErrorMessage => 
            "Apenas viagens em andamento podem ser finalizadas";
    }

    public class ViagemPodeSerCanceladaSpecification : Specification<Viagem>
    {
        public override bool IsSatisfiedBy(Viagem viagem)
        {
            return viagem.Situacao != SituacaoViagemEnum.Finalizada;
        }

        public override string ErrorMessage => 
            "Viagens finalizadas não podem ser canceladas";
    }

    public class ViagemPodeSerEditadaSpecification : Specification<Viagem>
    {
        public override bool IsSatisfiedBy(Viagem viagem)
        {
            return viagem.Situacao == SituacaoViagemEnum.Agendada;
        }

        public override string ErrorMessage => 
            "Apenas viagens agendadas podem ser editadas";
    }

    public class ViagemPodeReceberPosicaoSpecification : Specification<Viagem>
    {
        public override bool IsSatisfiedBy(Viagem viagem)
        {
            return viagem.Situacao == SituacaoViagemEnum.EmAndamento;
        }

        public override string ErrorMessage => 
            "Apenas viagens em andamento podem receber posições";
    }

    public class DataViagemValidaSpecification : Specification<DateTime>
    {
        public override bool IsSatisfiedBy(DateTime dataViagem)
        {
            return dataViagem.Date >= DateTime.Today;
        }

        public override string ErrorMessage => 
            "A data da viagem deve ser maior ou igual à data atual";
    }

    public class HorariosValidosSpecification : Specification<(TimeSpan saida, TimeSpan chegada)>
    {
        public override bool IsSatisfiedBy((TimeSpan saida, TimeSpan chegada) horarios)
        {
            return horarios.chegada > horarios.saida;
        }

        public override string ErrorMessage => 
            "O horário de chegada deve ser maior que o horário de saída";
    }
} 
