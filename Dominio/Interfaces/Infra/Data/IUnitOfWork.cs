namespace Dominio.Interfaces.Infra.Data
{
    public interface IUnitOfWork
    {
        int Commit();
        void RollBack();
    }
}
