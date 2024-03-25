using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.Services;

namespace DatalexionBackend.Infrastructure.Repositories
{
    public class WingRepository : Repository<Wing>, IWingRepository
    {
        private readonly ContextDB _dbContext;

        public WingRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Wing> Update(Wing entity)
        {
            entity.Update = DateTime.Now;
            _dbContext.Update(entity);
            await Save();
            return entity;
        }

        public IQueryable<Wing> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

    }
}