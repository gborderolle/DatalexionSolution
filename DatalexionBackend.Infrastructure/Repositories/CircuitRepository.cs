using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.Infrastructure.Repositories;

namespace Datalexion.Repository
{
    public class CircuitRepository : Repository<Circuit>, ICircuitRepository
    {
        private readonly DbContext _dbContext;

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