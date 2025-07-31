namespace Dominio.Interfaces.Infra.Data
{
    public interface IUnitOfWork
    {
        Task<int> Commit(CancellationToken cancellationToken = default);
        Task RollBack(CancellationToken cancellationToken = default);
    }
}
