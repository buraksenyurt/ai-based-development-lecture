using CvApp.Domain.Enums;

namespace CvApp.Application.DTOs;

public record ContactDto(
    Guid ContactId,
    ContactType Kind,
    Guid RelatedUser,
    string Value
);

public record CreateContactRequest(
    ContactType Kind,
    Guid RelatedUser,
    string Value
);

public record UpdateContactRequest(
    ContactType Kind,
    string Value
);
