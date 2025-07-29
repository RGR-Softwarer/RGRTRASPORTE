using Dominio.Entidades.Auditoria;
using Dominio.Entidades;
using Dominio.Enums.Auditoria;
using Dominio.Interfaces.Infra.Data;
using Dominio.Models;
using Dominio.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infra.CrossCutting.Services
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly IGenericRepository<RegistroAuditoria> _repository;
        private readonly ILogger<AuditoriaService> _logger;

        public AuditoriaService(
            IGenericRepository<RegistroAuditoria> repository,
            ILogger<AuditoriaService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task RegistrarAlteracaoAsync(
            string nomeEntidade,
            long entidadeId,
            AcaoBancoDadosEnum acao,
            object dadosOriginais,
            object dadosNovos,
            string usuario,
            string ip)
        {
            try
            {
                var dados = new
                {
                    Original = dadosOriginais,
                    Novo = dadosNovos,
                    Alteracoes = CalcularDiferencas(dadosOriginais, dadosNovos)
                };

                var registro = new RegistroAuditoria(
                    nomeEntidade,
                    entidadeId,
                    acao,
                    JsonSerializer.Serialize(dados),
                    usuario ?? "Sistema",
                    ip ?? "Unknown");

                await _repository.AdicionarAsync(registro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar auditoria para {NomeEntidade}({EntidadeId})", 
                    nomeEntidade, entidadeId);
            }
        }

        public async Task RegistrarEventoAsync(
            string evento,
            long entidadeId,
            string dadosEvento,
            string usuario,
            string ip)
        {
            try
            {
                var registro = new RegistroAuditoria(
                    evento,
                    entidadeId,
                    AcaoBancoDadosEnum.Evento,
                    dadosEvento,
                    usuario ?? "Sistema",
                    ip ?? "Unknown");

                await _repository.AdicionarAsync(registro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar evento {Evento} para entidade {EntidadeId}", 
                    evento, entidadeId);
            }
        }

        public async Task<IEnumerable<RegistroAuditoria>> ObterHistoricoAsync(
            string nomeEntidade,
            long entidadeId)
        {
            return await _repository.Query()
                .Where(r => r.NomeEntidade == nomeEntidade && r.EntidadeId == entidadeId)
                .OrderByDescending(r => r.DataOcorrencia)
                .ToListAsync();
        }

        private List<HistoricoPropriedade> CalcularDiferencas(object original, object novo)
        {
            if (original == null || novo == null)
                return new List<HistoricoPropriedade>();

            try
            {
                var propriedadesOriginais = original.GetType().GetProperties();
                var propriedadesNovas = novo.GetType().GetProperties();
                var diferencas = new List<HistoricoPropriedade>();

                foreach (var prop in propriedadesOriginais)
                {
                    var propNova = propriedadesNovas.FirstOrDefault(p => p.Name == prop.Name);
                    if (propNova != null)
                    {
                        var valorOriginal = prop.GetValue(original);
                        var valorNovo = propNova.GetValue(novo);

                        if (!Equals(valorOriginal, valorNovo))
                        {
                            diferencas.Add(new HistoricoPropriedade(
                                prop.Name,
                                valorOriginal?.ToString() ?? "null",
                                valorNovo?.ToString() ?? "null"
                            ));
                        }
                    }
                }

                return diferencas;
            }
            catch
            {
                return new List<HistoricoPropriedade>();
            }
        }
    }
} 