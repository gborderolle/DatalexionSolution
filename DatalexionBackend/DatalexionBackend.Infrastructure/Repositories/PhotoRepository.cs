using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace DatalexionBackend.Infrastructure.Repositories
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        private readonly ContextDB _dbContext;

        public PhotoRepository(ContextDB dbContext, ILogService logService) : base(dbContext)
        {
            _dbContext = dbContext;
        }

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public async Task<Photo> Update(Photo entity)
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            entity.Update = DateTime.Now;
            _dbContext.Update(entity);
            await Save();
            return entity;
        }

        public IQueryable<Photo> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

        public async Task<List<Photo>> FindPhotosByCircuitId(int circuitId)
        {
            return await _dbContext.Set<Photo>()
                                   .Where(photo => photo.CircuitId == circuitId)
                                   .ToListAsync();
        }

    }
}