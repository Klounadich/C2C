namespace Identity.DTO;

public record UserResponse(
    string Username,
    string AcessToken,
    string RefreshToken,
    DateTime CreatedAt
);