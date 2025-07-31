using Dominio.Entidades.Localidades;

namespace Dominio.Specifications
{
    public class LocalidadePodeSerAtivadaSpecification : Specification<Localidade>
    {
        public override bool IsSatisfiedBy(Localidade localidade)
        {
            return !localidade.Ativo;
        }

        public override string ErrorMessage => "Localidade já está ativa.";
    }

    public class LocalidadePodeSerInativadaSpecification : Specification<Localidade>
    {
        public override bool IsSatisfiedBy(Localidade localidade)
        {
            return localidade.Ativo;
        }

        public override string ErrorMessage => "Localidade já está inativa.";
    }

    public class LocalidadePodeSerEditadaSpecification : Specification<Localidade>
    {
        public override bool IsSatisfiedBy(Localidade localidade)
        {
            // Localidades podem sempre ser editadas (adicionar regras específicas se necessário)
            return true;
        }

        public override string ErrorMessage => "Localidade não pode ser editada.";
    }

    public class LocalidadeCoordenadaValidaSpecification : Specification<(decimal latitude, decimal longitude)>
    {
        public override bool IsSatisfiedBy((decimal latitude, decimal longitude) coordenadas)
        {
            return coordenadas.latitude >= -90 && coordenadas.latitude <= 90 &&
                   coordenadas.longitude >= -180 && coordenadas.longitude <= 180;
        }

        public override string ErrorMessage => "Coordenadas inválidas.";
    }
} 
