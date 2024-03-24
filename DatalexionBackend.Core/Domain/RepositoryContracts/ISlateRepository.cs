using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface ISlateRepository : IRepository<Slate>
    {
        Task<Slate> Update(Slate entity);
        IQueryable<Slate> GetAllQueryable();
    }
}