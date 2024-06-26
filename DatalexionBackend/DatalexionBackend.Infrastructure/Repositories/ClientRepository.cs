﻿using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.Services;

namespace DatalexionBackend.Infrastructure.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private readonly ContextDB _dbContext;

        public ClientRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Client> Update(Client entity)
        {
            entity.Update = DateTime.Now;
            _dbContext.Update(entity);
            await Save();
            return entity;
        }

        public IQueryable<Client> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

    }
}