namespace CvApp.Application.DTOs;

public record UserDto(
    Guid UserId,
    string Fullname
);

public record CreateUserRequest(
    string Fullname
);

public record UpdateUserRequest(
    string Fullname
);
