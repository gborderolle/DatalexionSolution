using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.Infrastructure.Repositories;

namespace Datalexion.Repository
{
    public class DatalexionUserRepository : Repository<DatalexionUser>, IDatalexionUserRepository
    {
        private readonly DbContext _dbContext;

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

    }
}