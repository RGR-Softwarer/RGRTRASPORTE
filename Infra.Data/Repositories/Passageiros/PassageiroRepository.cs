using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Interfaces.Infra.Data.Passageiros;
using Dominio.Interfaces.Infra.Data.Localidades;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Passageiros
{
    public class PassageiroRepository : GenericRepository<Passageiro>, IPassageiroRepository
    {
        private readonly ILocalidadeRepository _localidadeRepository;

        public PassageiroRepository(
            CadastroContext context,
            ILocalidadeRepository localidadeRepository) : base(context)
        {
            _localidadeRepository = localidadeRepository;
        }

        public async Task<Passageiro> ObterPassageiroCompletoAsync(long id)
        {
            var passageiro = await Query()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (passageiro != null)
            {
                passageiro.GetType().GetProperty("Localidade")?.SetValue(passageiro,
                    await _localidadeRepository.ObterPorIdAsync(passageiro.LocalidadeId));

                if (passageiro.LocalidadeEmbarqueId.HasValue)
                {
                    passageiro.GetType().GetProperty("LocalidadeEmbarque")?.SetValue(passageiro,
                        await _localidadeRepository.ObterPorIdAsync(passageiro.LocalidadeEmbarqueId.Value));
                }

                if (passageiro.LocalidadeDesembarqueId.HasValue)
                {
                    passageiro.GetType().GetProperty("LocalidadeDesembarque")?.SetValue(passageiro,
                        await _localidadeRepository.ObterPorIdAsync(passageiro.LocalidadeDesembarqueId.Value));
                }
            }

            return passageiro;
        }
    }
}