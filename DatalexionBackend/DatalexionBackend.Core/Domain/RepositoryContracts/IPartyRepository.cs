using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IPartyRepository : IRepository<Party>
    {
        Task<Party> Update(Party entity);
        IQueryable<Party> GetAllQueryable();
    }
}