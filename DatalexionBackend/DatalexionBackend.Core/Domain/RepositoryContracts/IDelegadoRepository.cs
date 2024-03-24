using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IDelegadoRepository : IRepository<Delegado>
    {
        Task<Delegado> Update(Delegado entity);
        IQueryable<Delegado> GetAllQueryable();
    }
}