using Dominio.Entidades.Veiculos;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Entidades.Viagens.Gatilho;

namespace Dominio.Specifications
{
    // Specifications simples para ModeloVeicular
    public class ModeloVeicularPodeSerEditadoSpecification : Specification<ModeloVeicular>
    {
        public override bool IsSatisfiedBy(ModeloVeicular modelo)
        {
            return true; // Modelos podem sempre ser editados
        }

        public override string ErrorMessage => "Modelo não pode ser editado";
    }



    public class ModeloVeicularPodeSerAtivadoSpecification : Specification<ModeloVeicular>
    {
        public override bool IsSatisfiedBy(ModeloVeicular modelo)
        {
            return !modelo.Situacao;
        }

        public override string ErrorMessage => "Modelo já está ativo";
    }

    // Specifications simples para Motorista
    public class MotoristaPodeSerEditadoSpecification : Specification<Motorista>
    {
        public override bool IsSatisfiedBy(Motorista motorista)
        {
            return motorista.Situacao;
        }

        public override string ErrorMessage => "Motorista inativo não pode ser editado";
    }



    public class MotoristaPodeAtualizarDocumentosSpecification : Specification<Motorista>
    {
        public override bool IsSatisfiedBy(Motorista motorista)
        {
            return motorista.Situacao;
        }

        public override string ErrorMessage => "Não é possível atualizar documentos de motorista inativo";
    }

    // Specifications simples para Passageiro
    public class PassageiroPodeSerEditadoSpecification : Specification<Passageiro>
    {
        public override bool IsSatisfiedBy(Passageiro passageiro)
        {
            return passageiro.Situacao;
        }

        public override string ErrorMessage => "Passageiro inativo não pode ser editado";
    }

    public class PassageiroPodeAtualizarLocalidadesSpecification : Specification<Passageiro>
    {
        public override bool IsSatisfiedBy(Passageiro passageiro)
        {
            return passageiro.Situacao;
        }

        public override string ErrorMessage => "Não é possível atualizar localidades de passageiro inativo";
    }

    public class PassageiroPodeSerAtivadoSpecification : Specification<Passageiro>
    {
        public override bool IsSatisfiedBy(Passageiro passageiro)
        {
            return !passageiro.Situacao;
        }

        public override string ErrorMessage => "Passageiro já está ativo";
    }





    // Specifications simples para GatilhoViagem
    public class GatilhoViagemPodeSerEditadoSpecification : Specification<GatilhoViagem>
    {
        public override bool IsSatisfiedBy(GatilhoViagem gatilho)
        {
            return gatilho.Ativo;
        }

        public override string ErrorMessage => "Gatilho inativo não pode ser editado";
    }

    public class GatilhoViagemPodeAtualizarHorariosSpecification : Specification<GatilhoViagem>
    {
        public override bool IsSatisfiedBy(GatilhoViagem gatilho)
        {
            return gatilho.Ativo;
        }

        public override string ErrorMessage => "Não é possível atualizar horários de gatilho inativo";
    }

    public class GatilhoViagemPodeAtualizarValorSpecification : Specification<GatilhoViagem>
    {
        public override bool IsSatisfiedBy(GatilhoViagem gatilho)
        {
            return gatilho.Ativo;
        }

        public override string ErrorMessage => "Não é possível atualizar valor de gatilho inativo";
    }

    public class GatilhoViagemPodeSerAtivadoSpecification : Specification<GatilhoViagem>
    {
        public override bool IsSatisfiedBy(GatilhoViagem gatilho)
        {
            return !gatilho.Ativo;
        }

        public override string ErrorMessage => "Gatilho já está ativo";
    }


}
