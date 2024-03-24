using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts;

public interface IParticipantRepository : IRepository<Participant>
{
    Task<Participant> Update(Participant entity);
    IQueryable<Participant> GetAllQueryable();
}