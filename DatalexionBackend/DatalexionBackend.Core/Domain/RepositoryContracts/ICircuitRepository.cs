using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface ICircuitRepository : IRepository<Circuit>
    {
        Task<Circuit> Update(Circuit entity);
        IQueryable<Circuit> GetAllQueryable();
    }
}