using CvApp.Domain.Aggregates;

namespace CvApp.Domain.Interfaces;

public interface IResumeRepository
{
    Task<Resume?> GetByIdAsync(Guid resumeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Resume>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Resume>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Resume resume, CancellationToken cancellationToken = default);
    Task UpdateAsync(Resume resume, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid resumeId, CancellationToken cancellationToken = default);
}
