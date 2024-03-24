﻿using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IDatalexionUserRepository : IRepository<DatalexionUser>
    {
        Task<DatalexionUser> Update(DatalexionUser entity);
        IQueryable<DatalexionUser> GetAllQueryable();
    }
}