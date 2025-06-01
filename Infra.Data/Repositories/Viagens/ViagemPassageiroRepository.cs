using Microsoft.EntityFrameworkCore;
using Dominio.Entidades.Viagens;
using Dominio.Interfaces.Infra.Data.Viagens;
using Dominio.Interfaces.Infra.Data.Passageiros;
using Infra.Data.Context;
using Infra.Data.Data;

namespace Infra.Data.Repositories.Viagens
{
    public class ViagemPassageiroRepository : GenericRepository<ViagemPassageiro>, IViagemPassageiroRepository
    {
        private readonly IPassageiroRepository _passageiroRepository;

        public ViagemPassageiroRepository(
            TransportadorContext context, 
            IPassageiroRepository passageiroRepository) : base(context)
        {
            _passageiroRepository = passageiroRepository;
        }

        public async Task<ViagemPassageiro> ObterViagemPassageiroCompletoAsync(long id)
        {
            var viagemPassageiro = await Query()
                .Include(vp => vp.Viagem)
                .FirstOrDefaultAsync(vp => vp.Id == id);

            if (viagemPassageiro != null)
            {
                viagemPassageiro.GetType().GetProperty("Passageiro")?.SetValue(viagemPassageiro,
                    await _passageiroRepository.ObterPorIdAsync(viagemPassageiro.PassageiroId));
            }

            return viagemPassageiro;
        }

        public async Task<IEnumerable<ViagemPassageiro>> ObterPassageirosPorViagemAsync(long viagemId)
        {
            var viagensPassageiros = await Query()
                .Include(vp => vp.Viagem)
                .Where(vp => vp.ViagemId == viagemId)
                .ToListAsync();

            foreach (var viagemPassageiro in viagensPassageiros)
            {
                viagemPassageiro.GetType().GetProperty("Passageiro")?.SetValue(viagemPassageiro,
                    await _passageiroRepository.ObterPorIdAsync(viagemPassageiro.PassageiroId));
            }

            return viagensPassageiros;
        }

        public async Task<IEnumerable<ViagemPassageiro>> ObterViagensPorPassageiroAsync(long passageiroId)
        {
            var viagensPassageiros = await Query()
                .Include(vp => vp.Viagem)
                .Where(vp => vp.PassageiroId == passageiroId)
                .ToListAsync();

            foreach (var viagemPassageiro in viagensPassageiros)
            {
                viagemPassageiro.GetType().GetProperty("Passageiro")?.SetValue(viagemPassageiro,
                    await _passageiroRepository.ObterPorIdAsync(viagemPassageiro.PassageiroId));
            }

            return viagensPassageiros;
        }
    }
}