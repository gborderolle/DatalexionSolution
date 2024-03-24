using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.Services;

namespace DatalexionBackend.Infrastructure.Repositories;

public class CandidateRepository : Repository<Candidate>, ICandidateRepository
{
    private readonly ContextDB _dbContext;

    public CandidateRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Candidate> Update(Candidate entity)
    {
        entity.Update = DateTime.Now;
        _dbContext.Update(entity);
        await Save();
        return entity;
    }

    public IQueryable<Candidate> GetAllQueryable()
    {
        return dbSet.AsQueryable();
    }

}