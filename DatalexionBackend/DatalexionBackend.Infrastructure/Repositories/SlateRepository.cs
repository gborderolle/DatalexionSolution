using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.Infrastructure.Repositories;

namespace Datalexion.Repository
{
    public class SlateRepository : Repository<Slate>, ISlateRepository
    {
        private readonly ContextDB _dbContext;

        public SlateRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Slate> Update(Slate entity)
        {
            entity.Update = DateTime.Now;
            _dbContext.Update(entity);
            await Save();
            return entity;
        }

        public IQueryable<Slate> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

    }
}