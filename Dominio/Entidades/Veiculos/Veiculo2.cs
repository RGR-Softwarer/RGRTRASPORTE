using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Veiculos
{
    internal class Veiculo2
    {

          public virtual long Codigo { get; set; }

        public virtual bool Situacao { get; set; }

        public virtual string Placa { get; set; }

        public virtual string NumeroChassi { get; set; }

        public virtual int NumeroAssentosPreferenciais { get; set; }

        public virtual string Renavam { get; set; }

        public virtual int AnoModelo { get; set; }

        public virtual int AnoFabricacao { get; set; }

        public virtual DateTime? VencimentoLicenciamento { get; set; }

        public virtual Dominio.Entidades.Veiculos.ModeloVeiculo ModeloVeiculo { get; set; }

//        public virtual Dominio.Entidades.Pessoas.Transportador Transportador { get; set; }

        public virtual bool PossuiBanheiro { get; set; }

        public virtual bool PossuiClimatizador { get; set; }

        public virtual bool PossuiRastreador { get; set; }

        public virtual int NumeroAssentos { get; set; }

        public virtual int CapacidadeMaximaPassageiros { get; set; }

        public virtual int NumeroPassageirosEmPe { get; set; }

        public virtual string Descricao
        {
            get { return $"{Placa}"; }
        }

        public virtual string DescricaoAtivo
        {
            get { return Situacao ? "Ativo" : "Inativo"; }
        }

        public virtual string PlacaFormatada
        {
            get { return string.IsNullOrWhiteSpace(Placa) ? string.Empty : $"{Placa.Substring(0, 3)}-{Placa.Substring(3, 4)}"; }
        }
    }
}
