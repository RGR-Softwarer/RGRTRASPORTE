using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Veiculos
{
    internal class ModeloVeiculo
    {

        
        public virtual long Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual bool Situacao { get; set; }

        //public virtual Enumeradores.Veiculos.TipoModeloVeiculo Tipo { get; set; }

        public virtual int QuantidadeAssento { get; set; }

        public virtual int QuantidadeEixo { get; set; }

        public virtual int CapacidadeMaxima { get; set; }

        public virtual int PassageirosEmPe { get; set; }

        public virtual bool PossuiBanheiro { get; set; }

        public virtual bool PossuiClimatizador { get; set; }

        public virtual string DescricaoAtivo
        {
            get { return Situacao ? "Ativo" : "Inativo"; }
        }
    }
}
