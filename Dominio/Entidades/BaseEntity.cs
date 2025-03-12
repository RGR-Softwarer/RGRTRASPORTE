using Dominio.Entidades.Auditoria;
using Dominio.Enums;
using System.Collections;
using System.Reflection;

namespace Dominio.Entidades
{
    public class BaseEntity
    {
        #region Variáveis Privadas

        private List<HistoricoPropriedade> _valoresAlterados = null;
        private List<HistoricoPropriedade> _valoresExternosAlterados = null;
        private Dictionary<string, object> _valoresOriginais = null;
        private bool _initialized = false;

        #endregion

        #region Construtores

        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        #endregion Construtores

        #region Propriedades

        public long Id { get; set; }

        protected virtual string DescricaoFormatada => Id.ToString();

        public virtual string DescricaoAuditoria
        {
            get { return DescricaoFormatada; }
        }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        #endregion



        #region Métodos Globais

        public virtual T Clonar<T>()
        {
            return (T)this.MemberwiseClone();
        }

        public virtual HistoricoPropriedade GetChangeByPropertyName(string nomePropriedade)
        {
            return GetChangesByPropertyName(nomePropriedade).FirstOrDefault();
        }

        public virtual List<HistoricoPropriedade> GetChangesByPropertyName(string nomePropriedade)
        {
            List<HistoricoPropriedade> valoresAlterados = ObterValoresAlterados();

            return valoresAlterados.Where(o => o.Propriedade == nomePropriedade).ToList();
        }

        /// <summary>
        /// Obtém as informações de alterações realizadas nas propriedades do objeto
        /// </summary>
        public virtual List<Auditoria.HistoricoPropriedade> GetChanges()
        {
            if (_valoresAlterados == null)
                CarregarValoresAlterados();

            return _valoresAlterados;
        }

        public virtual List<HistoricoPropriedade> GetCurrentChanges()
        {
            return ObterValoresAlterados();
        }

        /// <summary>
        /// Salva as informações do objeto no estado atual para auditar as alterações posteriormente pelo método GetChanges
        /// </summary>
        public virtual void Initialize()
        {
            if (_initialized)
                return;

            CarregarValoresOriginais();

            _valoresExternosAlterados = new List<HistoricoPropriedade>();
            _initialized = true;
        }

        public virtual bool IsChanged()
        {
            List<HistoricoPropriedade> valoresAlterados = ObterValoresAlterados();

            return valoresAlterados.Count > 0;
        }

        public virtual bool IsChangedByPropertyName(string nomePropriedade)
        {
            return GetChangesByPropertyName(nomePropriedade).Count > 0;
        }

        public virtual bool IsInitialized()
        {
            return _initialized;
        }

        public virtual void SetChanges()
        {
            CarregarValoresAlterados();
        }

        public virtual void SetExternalChange(HistoricoPropriedade valorAlterado)
        {
            if (valorAlterado == null)
                return;

            SetExternalChanges(new List<HistoricoPropriedade>() { valorAlterado });
        }

        public virtual void SetExternalChanges(List<HistoricoPropriedade> valoresAlterados)
        {
            if (!_initialized)
                return;

            if ((valoresAlterados == null) || (valoresAlterados.Count == 0))
                return;

            _valoresExternosAlterados.AddRange(valoresAlterados);
        }

        public virtual List<HistoricoPropriedade> GetExternalChanges()
        {
            if (_valoresExternosAlterados == null)
                return new List<HistoricoPropriedade>();

            return _valoresExternosAlterados;
        }

        #endregion

        #region Métodos Privados

        private void CarregarValoresAlterados()
        {
            if (!_initialized)
                return;

            List<HistoricoPropriedade> valoresAlterados = ObterValoresAlterados();
            _valoresAlterados = new List<Auditoria.HistoricoPropriedade>();

            foreach (HistoricoPropriedade valorAlterado in valoresAlterados)
                _valoresAlterados.Add(new Auditoria.HistoricoPropriedade(valorAlterado.Propriedade, valorAlterado.De, valorAlterado.Para));
        }

        private void CarregarValoresOriginais()
        {
            _valoresOriginais = new Dictionary<string, object>();
            PropertyInfo[] propriedades = this.GetType().GetProperties();

            foreach (PropertyInfo property in propriedades)
            {
                if (!property.CanWrite)
                    continue;

                dynamic valor = property.GetValue(this, null);

                if (valor != null && IsEntityFrameworkSet(valor, valor.GetType()))
                {
                    List<dynamic> valoresOriginais = valor.ToList();

                    _valoresOriginais.Add(property.Name, valoresOriginais);

                    continue;
                }

                this._valoresOriginais.Add(property.Name, valor);
            }
        }

        private bool IsEntidade(Type tipoPropriedade)
        {
            return tipoPropriedade.IsClass && (tipoPropriedade.Name != "String") && !tipoPropriedade.IsEnum;
        }

        private bool IsList(object valor, Type tipoPropriedade)
        {
            if (valor == null)
                return false;

            return valor is IList && tipoPropriedade.IsGenericType &&
                   (tipoPropriedade.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>)) ||
                    tipoPropriedade.GetGenericTypeDefinition().IsAssignableFrom(typeof(HashSet<>)) ||
                    tipoPropriedade.GetGenericTypeDefinition().IsAssignableFrom(typeof(ICollection<>)));
        }

        private bool IsEntityFrameworkSet(object valor, Type tipoPropriedade)
        {
            if (valor == null)
                return false;

            return tipoPropriedade.IsGenericType &&
                   (tipoPropriedade.GetGenericTypeDefinition() == typeof(HashSet<>) ||
                    tipoPropriedade.GetGenericTypeDefinition() == typeof(ICollection<>));
        }


        private List<HistoricoPropriedade> ObterItensAlterados(PropertyInfo propriedade, dynamic valorOriginal, dynamic valorAtual)
        {
            List<HistoricoPropriedade> itensAlterados = new List<HistoricoPropriedade>();
            Type tipo = null;
            bool tipoEntidade = false;

            if (valorOriginal != null)
            {
                foreach (dynamic itemValorOriginal in valorOriginal ?? Enumerable.Empty<object>())
                {
                    bool existeItem = false;

                    if (tipo == null)
                    {
                        tipo = itemValorOriginal.GetType();
                        tipoEntidade = IsEntidade(tipo);
                    }

                    if (valorAtual != null)
                    {
                        foreach (dynamic itemValorAtual in valorAtual ?? Enumerable.Empty<object>())
                        {
                            if ((tipoEntidade && itemValorOriginal.Id == itemValorAtual.Id) || (!tipoEntidade && itemValorOriginal == itemValorAtual))
                            {
                                existeItem = true;
                                break;
                            }
                        }
                    }

                    if (!existeItem)
                        itensAlterados.Add(new HistoricoPropriedade()
                        {
                            Propriedade = propriedade.Name,
                            De = ObterValorPropriedadeFormatado(itemValorOriginal, tipo),
                            Para = ""
                        });
                }
            }

            if (valorAtual != null)
            {
                foreach (dynamic itemValorAtual in valorAtual ?? Enumerable.Empty<object>())
                {
                    bool existeItem = false;

                    if (tipo == null)
                    {
                        tipo = itemValorAtual.GetType();
                        tipoEntidade = IsEntidade(tipo);
                    }

                    if (valorOriginal != null)
                    {
                        foreach (dynamic itemFrom in valorOriginal ?? Enumerable.Empty<object>())
                        {
                            if ((tipoEntidade && itemFrom.Id == itemValorAtual.Id) || (!tipoEntidade && itemFrom == itemValorAtual))
                            {
                                existeItem = true;
                                break;
                            }
                        }
                    }

                    if (!existeItem)
                        itensAlterados.Add(new HistoricoPropriedade()
                        {
                            Propriedade = propriedade.Name,
                            De = "",
                            Para = ObterValorPropriedadeFormatado(itemValorAtual, tipo)
                        });
                }
            }

            return itensAlterados;
        }

        private string ObterValorPropriedadeFormatado(dynamic valor, Type tipoPropriedade)
        {
            if (valor == null)
                return string.Empty;

            if (tipoPropriedade.IsClass && (tipoPropriedade.Name != "String") && !tipoPropriedade.IsEnum)
                return $"{valor.Id.ToString()} - {(valor.Descricao ?? string.Empty)}";

            if (tipoPropriedade.IsEnum)
            {
                var enumerador = ((Enum)valor);
                return $"{enumerador:D} - {enumerador:G}";
            }

            if (tipoPropriedade == typeof(string))
                return (string)valor;

            if (tipoPropriedade == typeof(int) || tipoPropriedade == typeof(long))
                return ((long)valor).ToString("n0");

            if (tipoPropriedade == typeof(decimal) || tipoPropriedade == typeof(double) || tipoPropriedade == typeof(float))
                return ((decimal)valor).ToString("n6");

            if (tipoPropriedade == typeof(bool))
                return ((bool)valor) ? "Sim" : "Não";

            if (tipoPropriedade == typeof(DateTime))
                return ((DateTime)valor).ToString("dd/MM/yyyy HH:mm:ss");

            if (tipoPropriedade == typeof(TimeSpan))
                return ((TimeSpan)valor).ToString(@"hh\:mm");

            return string.Empty;
        }

        private List<HistoricoPropriedade> ObterValoresAlterados()
        {
            List<HistoricoPropriedade> valoresAlterados = new List<HistoricoPropriedade>();

            if (!_initialized)
                return valoresAlterados;

            PropertyInfo[] propriedades = this.GetType().GetProperties().ToArray();

            for (int i = 0; i < propriedades.Length; i++)
            {
                PropertyInfo propriedade = propriedades[i];

                if (!propriedade.CanWrite)
                    continue;

                dynamic valorOriginal = _valoresOriginais[propriedade.Name];
                dynamic valorAtual = propriedade.GetValue(this, null);
                bool valorAlterado;

                try
                {
                    valorAlterado = !Equals(valorAtual, valorOriginal);
                }
                catch
                {
                    valorAlterado = true;
                }

                if (!valorAlterado)
                    continue;

                Type tipo = valorAtual?.GetType() ?? valorOriginal.GetType();

                //Quando é lista, utiliza só para obter os dados, então não acessa a propriedade
                if (IsList(valorAtual, tipo))
                    continue;

                if (IsEntityFrameworkSet(valorAtual, tipo))
                {
                    valoresAlterados.AddRange(ObterItensAlterados(propriedade, valorOriginal, valorAtual));
                    continue;
                }

                if (IsEntidade(tipo) && ((valorOriginal == null && valorAtual == null) || (valorOriginal != null && valorAtual != null && valorOriginal.Id == valorAtual.Id)))
                    continue;

                valoresAlterados.Add(new HistoricoPropriedade()
                {
                    Propriedade = propriedade.Name,
                    De = ObterValorPropriedadeFormatado(valorOriginal, valorOriginal?.GetType() ?? tipo),
                    Para = ObterValorPropriedadeFormatado(valorAtual, tipo)
                });
            }

            valoresAlterados.AddRange(_valoresExternosAlterados);

            return valoresAlterados;
        }

        #endregion
    }
}
