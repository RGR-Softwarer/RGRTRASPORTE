using Dominio.Dtos;
using Dominio.Entidades;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Context;
using Infra.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class VeiculoRepository : GenericRepository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(RGRContext context) : base(context) { }

        public async Task<List<Veiculo>> ObterTodosAsync()
        {
            var veiculos = await Query().ToListAsync();

            return veiculos;
        }

        public async Task<Veiculo> ObterPorIdAsync(int id)
        {
            var veiculo = await Query().FirstOrDefaultAsync(x => x.Id == id);

            return veiculo;
        }

        public async Task AdicionarAsync(Veiculo veiculo)
        {
            await AddAsync(veiculo);
        }

        public async Task AdicionarEmLoteAsync(List<Veiculo> veiculo)
        {
            await AddManyAsync(veiculo);
        }

        public void Editar(Veiculo veiculo)
        {
            Update(veiculo);
        }

        public void EditarEmLoteAsync(List<Veiculo> dto)
        {
            UpdateMany(dto);
        }

        public void Remover(Veiculo veiculo)
        {
            Remove(veiculo);
        }
       
    }
}
