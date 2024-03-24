using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Infrastructure.Repositories;

public class ParticipantRepository : Repository<Participant>, IParticipantRepository
{
    private readonly DbContext _dbContext;

    public ParticipantRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Participant> Update(Participant entity)
    {
        entity.Update = DateTime.Now;
        _dbContext.Update(entity);
        await Save();
        return entity;
    }

    public IQueryable<Participant> GetAllQueryable()
    {
        return dbSet.AsQueryable();
    }

}