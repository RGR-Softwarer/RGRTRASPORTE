namespace Dominio.Interfaces.Infra.Data
{
    public interface IUnitOfWork
    {
        Task<int> Commit();
        Task RollBack();
    }
}
