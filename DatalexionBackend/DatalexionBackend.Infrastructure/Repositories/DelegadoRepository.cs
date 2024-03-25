﻿using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.Services;

namespace DatalexionBackend.Infrastructure.Repositories
{
    public class DelegadoRepository : Repository<Delegado>, IDelegadoRepository
    {
        private readonly ContextDB _dbContext;

        public DelegadoRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Delegado> Update(Delegado entity)
        {
            entity.Update = DateTime.Now;
            _dbContext.Update(entity);
            await Save();
            return entity;
        }

        public IQueryable<Delegado> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

    }
}