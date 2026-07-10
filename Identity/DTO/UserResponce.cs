namespace Identity.DTO;

public record UserResponse(
    string Username,
    string Token,
    DateTime CreatedAt
);