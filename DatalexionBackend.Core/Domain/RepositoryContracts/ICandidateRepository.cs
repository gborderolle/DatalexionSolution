using DatalexionBackend.Core.Domain.Entities;

namespace DatalexionBackend.Core.Domain.RepositoryContracts;

public interface ICandidateRepository : IRepository<Candidate>
{
    Task<Candidate> Update(Candidate entity);
    IQueryable<Candidate> GetAllQueryable();
}