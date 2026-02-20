using CvApp.Application.DTOs;
using CvApp.Application.Interfaces;
using CvApp.Domain.Aggregates;
using CvApp.Domain.Entities;
using CvApp.Domain.Interfaces;

namespace CvApp.Application.Services;

public class ResumeService : IResumeService
{
    private readonly IResumeRepository _resumeRepository;

    public ResumeService(IResumeRepository resumeRepository)
    {
        _resumeRepository = resumeRepository;
    }

    public async Task<ResumeDto?> GetByIdAsync(Guid resumeId, CancellationToken cancellationToken = default)
    {
        var resume = await _resumeRepository.GetByIdAsync(resumeId, cancellationToken);
        return resume is null ? null : MapToDto(resume);
    }

    public async Task<IEnumerable<ResumeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var resumes = await _resumeRepository.GetAllAsync(cancellationToken);
        return resumes.Select(MapToDto);
    }

    public async Task<ResumeDto> CreateAsync(CreateResumeRequest request, CancellationToken cancellationToken = default)
    {
        var user = User.Create(request.UserFullname);
        var resume = Resume.Create(user);

        await _resumeRepository.AddAsync(resume, cancellationToken);
        return MapToDto(resume);
    }

    public async Task DeleteAsync(Guid resumeId, CancellationToken cancellationToken = default)
    {
        await _resumeRepository.DeleteAsync(resumeId, cancellationToken);
    }

    public async Task AddContactAsync(Guid resumeId, CreateContactRequest request, CancellationToken cancellationToken = default)
    {
        var resume = await _resumeRepository.GetByIdAsync(resumeId, cancellationToken)
            ?? throw new InvalidOperationException($"Resume bulunamadı: {resumeId}");

        var contact = Contact.Create(request.Kind, request.RelatedUser, request.Value);
        resume.AddContact(contact);

        await _resumeRepository.UpdateAsync(resume, cancellationToken);
    }

    public async Task RemoveContactAsync(Guid resumeId, Guid contactId, CancellationToken cancellationToken = default)
    {
        var resume = await _resumeRepository.GetByIdAsync(resumeId, cancellationToken)
            ?? throw new InvalidOperationException($"Resume bulunamadı: {resumeId}");

        resume.RemoveContact(contactId);
        await _resumeRepository.UpdateAsync(resume, cancellationToken);
    }

    private static ResumeDto MapToDto(Resume resume)
    {
        var userDto = new UserDto(resume.User.UserId, resume.User.Fullname);

        var contactDtos = resume.Contacts
            .Select(c => new ContactDto(c.ContactId, c.Kind, c.RelatedUser, c.Value))
            .ToList()
            .AsReadOnly();

        return new ResumeDto(resume.ResumeId, userDto, contactDtos);
    }
}
