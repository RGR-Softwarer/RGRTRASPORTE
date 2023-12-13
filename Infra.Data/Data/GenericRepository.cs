using Dominio.Entidades;
using Dominio.Interfaces.Infra.Data;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected RGRContext _context;

        public GenericRepository(RGRContext context)
        {
            _context = context;
        }

        public IQueryable<T> Query() => _context.Set<T>().AsQueryable();


        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddManyAsync(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateMany(List<T> entity)
        {
            _context.Set<T>().AttachRange(entity);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
