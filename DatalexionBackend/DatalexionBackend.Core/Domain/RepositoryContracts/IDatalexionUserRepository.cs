using System.Linq.Expressions;
using DatalexionBackend.Core.Domain.IdentityEntities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IDatalexionUserRepository : IRepository<DatalexionUser>
    {
        Task<DatalexionUser> Update(DatalexionUser entity);
        IQueryable<DatalexionUser> GetAllQueryable();
        Task<bool> Exists(Expression<Func<DatalexionUser, bool>> predicate);
    }
}