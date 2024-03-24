using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IMunicipalityRepository : IRepository<Municipality>
    {
        Task<Municipality> Update(Municipality entity);
        IQueryable<Municipality> GetAllQueryable();
    }
}