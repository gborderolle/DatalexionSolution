using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.Services;

namespace DatalexionBackend.Infrastructure.Repositories
{
    public class CircuitRepository : Repository<Circuit>, ICircuitRepository
    {
        private readonly ContextDB _dbContext;

        public CircuitRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Circuit> Update(Circuit entity)
        {
            entity.Update = DateTime.Now;
            _dbContext.Update(entity);
            await Save();
            return entity;
        }

        public IQueryable<Circuit> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

    }
}