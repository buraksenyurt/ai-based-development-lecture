using CvApp.Application.DTOs;

namespace CvApp.Application.Interfaces;

public interface IResumeService
{
    Task<ResumeDto?> GetByIdAsync(Guid resumeId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ResumeDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ResumeDto> CreateAsync(CreateResumeRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid resumeId, CancellationToken cancellationToken = default);
    Task AddContactAsync(Guid resumeId, CreateContactRequest request, CancellationToken cancellationToken = default);
    Task RemoveContactAsync(Guid resumeId, Guid contactId, CancellationToken cancellationToken = default);
}
