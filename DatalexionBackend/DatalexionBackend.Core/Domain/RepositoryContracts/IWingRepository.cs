using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IWingRepository : IRepository<Wing>
    {
        Task<Wing> Update(Wing entity);
        IQueryable<Wing> GetAllQueryable();
    }
}