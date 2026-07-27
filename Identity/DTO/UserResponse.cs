namespace Identity.DTO;

// RequiresTwoFactor / CodeId — новые поля для двухшагового логина.
// Когда RequiresTwoFactor == true, AcessToken/RefreshToken пустые —
// куки на этом шаге НЕ выставляются (см. UserController.AuthUser).
public record UserResponse(
    string Username,
    string AcessToken,
    string RefreshToken,
    DateTime Date,
    bool RequiresTwoFactor = false,
    string? CodeId = null
);
