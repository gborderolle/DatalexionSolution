using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> Update(Client entity);
        IQueryable<Client> GetAllQueryable();
    }
}