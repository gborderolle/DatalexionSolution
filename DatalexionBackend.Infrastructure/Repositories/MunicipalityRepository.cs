using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.Infrastructure.Repositories;

namespace Datalexion.Repository
{
    public class MunicipalityRepository : Repository<Municipality>, IMunicipalityRepository
    {
        private readonly DbContext _dbContext;

        public MunicipalityRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Municipality> Update(Municipality entity)
        {
            entity.Update = DateTime.Now;
            _dbContext.Update(entity);
            await Save();
            return entity;
        }

        public IQueryable<Municipality> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

    }
}