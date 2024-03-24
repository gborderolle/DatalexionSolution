using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IProvinceRepository : IRepository<Province>
    {
        Task<Province> Update(Province entity);
        IQueryable<Province> GetAllQueryable();
    }
}