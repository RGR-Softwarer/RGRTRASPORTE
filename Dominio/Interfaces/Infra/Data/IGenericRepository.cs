namespace Dominio.Interfaces.Infra.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task AddAsync(T entity);
        Task AddManyAsync(List<T> entity);
        void Update(T entity);
        void UpdateMany(List<T> entity);
        void Remove(T entity);
    }
}
