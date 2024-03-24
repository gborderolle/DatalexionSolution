using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts
{
    public interface IPhotoRepository : IRepository<Photo>
    {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        Task<Photo> Update(Photo entity);
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        IQueryable<Photo> GetAllQueryable();
        Task<List<Photo>> FindPhotosByCircuitId(int circuitId);

    }
}

