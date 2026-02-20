namespace CvApp.Application.DTOs;

public record ResumeDto(
    Guid ResumeId,
    UserDto User,
    IReadOnlyCollection<ContactDto> Contacts
);

public record CreateResumeRequest(
    string UserFullname
);
