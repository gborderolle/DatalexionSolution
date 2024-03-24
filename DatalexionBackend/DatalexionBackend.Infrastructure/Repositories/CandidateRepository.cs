using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.Core.Domain.RepositoryContracts;

namespace DatalexionBackend.Infrastructure.Repositories;

public class CandidateRepository : Repository<Candidate>, ICandidateRepository
{
    private readonly DbContext _dbContext;

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