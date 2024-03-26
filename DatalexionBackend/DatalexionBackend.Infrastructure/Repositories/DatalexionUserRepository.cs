using System.Linq.Expressions;
using DatalexionBackend.Core.Domain.IdentityEntities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace DatalexionBackend.Infrastructure.Repositories
{
    public class DatalexionUserRepository : Repository<DatalexionUser>, IDatalexionUserRepository
    {
        private readonly ContextDB _dbContext;

        public DatalexionUserRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DatalexionUser> Update(DatalexionUser entity)
        {
            entity.Update = DateTime.Now;
            _dbContext.Update(entity);
            await Save();
            return entity;
        }

        public IQueryable<DatalexionUser> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

        public async Task<bool> Exists(Expression<Func<DatalexionUser, bool>> predicate)
        {
            return await _dbContext.DatalexionUser.AnyAsync(predicate);
        }

    }
}